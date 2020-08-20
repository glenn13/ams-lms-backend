using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using lms.Data;
using lms.Models;
using lms.Data.Repositories;
using lms.Data.Helpers;
using lms.Data.Services;
using lms.Data.Core;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOutlineController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseOutlineRepository _courseOutlineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validationService;
        private readonly IFileDirectory _fileDirectory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static object _NotFound;
        private static object _UnAuthorized;


        public CourseOutlineController(lmsContext context, 
                                       ICourseOutlineRepository courseOutlineRepository, 
                                       IUserRepository userRepository, 
                                       IValidationService validationService, 
                                       IFileDirectory fileDirectory,
                                       IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _courseOutlineRepository = courseOutlineRepository;
            _userRepository = userRepository;
            _validationService = validationService;
            _fileDirectory = fileDirectory;
            _hostingEnvironment = hostingEnvironment;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _UnAuthorized = new GenericResult { Response = false, Message = "You dont have a permission to access this module" };
        }

        // GET: api/CourseOutline
        [HttpGet("{courseId}")]
        public IActionResult List(long courseId)
        {
            try
            {
                var output = _courseOutlineRepository.GetAllByCourseId(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Course Outline is empty" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/CourseOutline/5
        [HttpGet("{id}/edit")]
        public IActionResult Edit(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                GenericResult _result = new GenericResult();

                var output = _courseOutlineRepository.GetById(id);

                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT: api/CourseOutline/5
        [HttpPut("{id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update(long id, [FromForm] CourseOutlineRequest request)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canModify == false)
                    return Unauthorized(_UnAuthorized);

                var model = _context.CourseOutline.Where(x => x.id == id).FirstOrDefault();
                model.title = request.title;
                model.userGroupId = request.userGroupId;
                model.visibility = request.visibility;
                model.duration = request.duration;
                model.description = request.description;

                if (request.title == null)
                    return BadRequest(new RequestValidationModel { Name = "Course Outline Title", Parameter = "title", Message = "Please enter course outline title" });


                var checkExists = _context.CourseOutline.Where(x => x.title == request.title && x.courseId == request.courseId && x.id != id).Any();
                if (checkExists == true)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " is already taken" });


                if (request.featureImage != null)
                {
                    if (request.featureImage.Length > 0)
                    {

                        Stream stream = request.featureImage.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseOutlineImageFolder = String.Format("{0}\\Content\\Images\\CourseOutline", path);

                        if (!Directory.Exists(courseOutlineImageFolder))
                            Directory.CreateDirectory(courseOutlineImageFolder);

                        var featureImageId = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureImage.FileName);
                        var fileName = featureImageId.ToString() + extension.ToString().ToLower();


                        if (System.IO.File.Exists(Path.Combine(courseOutlineImageFolder, fileName)))
                        {
                            System.IO.File.Delete(Path.Combine(courseOutlineImageFolder, fileName));
                        }

                        using (var zipStream = new FileStream(Path.Combine(courseOutlineImageFolder, fileName), FileMode.Create))
                        {
                            request.featureImage.CopyTo(zipStream);
                        }
                        model.featureImage = fileName;

                    }
                }

                if (request.interactiveVideo != null)
                {
                    if (request.interactiveVideo.Length > 0)
                    {
                        Stream stream = request.interactiveVideo.OpenReadStream();


                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseOutlineVideoFolder = String.Format("{0}\\Content\\Video\\CourseOutline", path);

                        if (!Directory.Exists(courseOutlineVideoFolder))
                            Directory.CreateDirectory(courseOutlineVideoFolder);

                        var idVideo = Guid.NewGuid();
                        var extension = Path.GetExtension(request.interactiveVideo.FileName);
                        var fileNameVideo = idVideo.ToString() + extension.ToString().ToLower();

                        model.interactiveVideo = idVideo.ToString();


                        if (System.IO.File.Exists(Path.Combine(courseOutlineVideoFolder, fileNameVideo)))
                        {
                            System.IO.File.Delete(Path.Combine(courseOutlineVideoFolder, fileNameVideo));
                        }


                        using (var zipStream = new FileStream(Path.Combine(courseOutlineVideoFolder, fileNameVideo), FileMode.Create))
                        {
                            request.interactiveVideo.CopyTo(zipStream);
                            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                            {
                                archive.ExtractToDirectory(Path.Combine(courseOutlineVideoFolder, idVideo.ToString()));

                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();



                //var response = _courseOutlineRepository.Update(id, request);

                // start: Course Outline Prerequisite
                if (request.CourseOutlinePrerequisite != null)
                {
                    for (int copKey = 0; request.CourseOutlinePrerequisite.Count() > copKey; copKey++)
                    {
                        CourseOutlinePrerequisite copModel = new CourseOutlinePrerequisite();
                        copModel.courseOutlineId = model.id;
                        copModel.courseId = model.courseId;
                        copModel.preRequisiteId = request.CourseOutlinePrerequisite[copKey].preRequisiteId;
                        _context.CourseOutlinePrerequisite.Add(copModel);
                        await _context.SaveChangesAsync();
                    }
                }



                // start: Course Outline Milestone
                if (request.CourseOutlineMilestone != null)
                {
                    for (int comKey = 0; request.CourseOutlineMilestone.Count() > comKey; comKey++)
                    {
                        CourseOutlineMilestone comModel = new CourseOutlineMilestone();
                        comModel.courseOutlineId = model.id;
                        comModel.courseId = model.courseId;
                        comModel.name = request.CourseOutlineMilestone[comKey].name;
                        comModel.lessonCompleted = request.CourseOutlineMilestone[comKey].lessonCompleted;

                        if (request.CourseOutlineMilestoneResourceFile != null)
                        {
                            if (request.CourseOutlineMilestoneResourceFile[comKey].Length > 0)
                            {
                                Stream stream = request.CourseOutlineMilestoneResourceFile[comKey].OpenReadStream();

                                var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                                string courseOutlineMilestoneVideoFolder = String.Format("{0}\\Content\\Images\\CourseOutlineMilestone", path);

                                if (!Directory.Exists(courseOutlineMilestoneVideoFolder))
                                    Directory.CreateDirectory(courseOutlineMilestoneVideoFolder);

                                var milestoneId = Guid.NewGuid();

                                var extension = Path.GetExtension(request.CourseOutlineMilestoneResourceFile[comKey].FileName);
                                var milestoneFileName = milestoneId.ToString() + extension.ToString().ToLower();


                                using (var zipStream = new FileStream(Path.Combine(courseOutlineMilestoneVideoFolder, milestoneFileName), FileMode.Create))
                                {
                                    request.CourseOutlineMilestoneResourceFile[comKey].CopyTo(zipStream);
                                }
                                comModel.resourceFile = milestoneFileName;
                            }
                        }
                        _context.CourseOutlineMilestone.Add(comModel);
                        await _context.SaveChangesAsync();
                    }
                }

                // start: Course Outline Media
                if (request.CourseOutlineMediaFile != null)
                {
                    for (int cofKey = 0; request.CourseOutlineMediaFile.Count() > cofKey; cofKey++)
                    {
                        CourseOutlineMedia cofModel = new CourseOutlineMedia();
                        cofModel.courseOutlineId = model.id;
                        cofModel.courseId = model.courseId;


                        if (request.CourseOutlineMediaFile[cofKey].Length > 0)
                        {
                            Stream stream = request.CourseOutlineMediaFile[cofKey].OpenReadStream();

                            var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                            string courseOutlineMediaFolder = String.Format("{0}\\Content\\Images\\CourseOutlineMedia", path);

                            if (!Directory.Exists(courseOutlineMediaFolder))
                                Directory.CreateDirectory(courseOutlineMediaFolder);

                            var mediaId = Guid.NewGuid();

                            var extension = Path.GetExtension(request.CourseOutlineMediaFile[cofKey].FileName);
                            var mediaFileName = mediaId.ToString() + extension.ToString().ToLower();


                            using (var zipStream = new FileStream(Path.Combine(courseOutlineMediaFolder, mediaFileName), FileMode.Create))
                            {
                                request.CourseOutlineMediaFile[cofKey].CopyTo(zipStream);
                            }
                            cofModel.resourceFile = mediaFileName;
                        }

                        _context.CourseOutlineMedia.Add(cofModel);
                        await _context.SaveChangesAsync();
                    }

                }
                return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully updated" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }   

        // POST: api/CourseOutline
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Store([FromForm] CourseOutlineRequest request)
        {

            try
            {
                if (_userRepository.LogCurrentUser().canCreate == false)
                    return Unauthorized(_UnAuthorized);


                CourseOutline model = new CourseOutline();
                model.title = request.title;
                model.courseId = request.courseId;
                model.userGroupId = request.userGroupId;
                model.visibility = request.visibility;
                model.duration = request.duration;
                model.description = request.description;
                model.Course = request.Course;
                model.UserGroup = request.UserGroup;


                RequiredFields modelRequest = new RequiredFields();
                modelRequest.CourseOutline = model;
                object validateFields = _validationService.ValidateRequest("Course Outline", modelRequest);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);


                var checkExists = _context.CourseOutline.Where(x => x.title == request.title && x.courseId == request.courseId).Any();
                if (checkExists == true)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " is already exists" });


                if (request.featureImage != null)
                {
                    if (request.featureImage.Length > 0)
                    {

                        Stream stream = request.featureImage.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseOutlineImageFolder = String.Format("{0}\\Content\\Images\\CourseOutline", path);

                        if (!Directory.Exists(courseOutlineImageFolder))
                            Directory.CreateDirectory(courseOutlineImageFolder);

                        var id = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureImage.FileName);
                        var fileName = id.ToString() + extension.ToString().ToLower();


                        using (var zipStream = new FileStream(Path.Combine(courseOutlineImageFolder, fileName), FileMode.Create))
                        {
                            request.featureImage.CopyTo(zipStream);
                        }
                        model.featureImage = fileName;

                    }
                }

                if (request.interactiveVideo != null)
                {
                    if (request.interactiveVideo.Length > 0)
                    {
                        Stream stream = request.interactiveVideo.OpenReadStream();


                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseOutlineVideoFolder = String.Format("{0}\\Content\\Video\\CourseOutline", path);

                        if (!Directory.Exists(courseOutlineVideoFolder))
                            Directory.CreateDirectory(courseOutlineVideoFolder);

                        var idVideo = Guid.NewGuid();
                        var extension = Path.GetExtension(request.interactiveVideo.FileName);
                        var fileNameVideo = idVideo.ToString() + extension.ToString().ToLower();

                        model.interactiveVideo = idVideo.ToString();


                        using (var zipStream = new FileStream(Path.Combine(courseOutlineVideoFolder, fileNameVideo), FileMode.Create))
                        {
                            request.interactiveVideo.CopyTo(zipStream);
                            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                            {
                                archive.ExtractToDirectory(Path.Combine(courseOutlineVideoFolder, idVideo.ToString()));

                            }
                        }
                    }
                }
                _context.CourseOutline.Add(model);
                await _context.SaveChangesAsync();



                // start: Course Outline Prerequisite
                if (request.CourseOutlinePrerequisite != null)
                {
                    for (int copKey = 0; request.CourseOutlinePrerequisite.Count() > copKey; copKey++)
                    {
                        CourseOutlinePrerequisite copModel = new CourseOutlinePrerequisite();
                        copModel.courseOutlineId = model.id;
                        copModel.courseId = request.courseId;
                        copModel.preRequisiteId = request.CourseOutlinePrerequisite[copKey].preRequisiteId;
                        _context.CourseOutlinePrerequisite.Add(copModel);
                        await _context.SaveChangesAsync();
                    }

                }

                // start: Course Outline Milestone
                if (request.CourseOutlineMilestone != null)
                {
                    for (int comKey = 0; request.CourseOutlineMilestone.Count() > comKey; comKey++)
                    {
                        CourseOutlineMilestone comModel = new CourseOutlineMilestone();
                        comModel.courseOutlineId = model.id;
                        comModel.courseId = request.courseId;
                        comModel.name = request.CourseOutlineMilestone[comKey].name;
                        comModel.lessonCompleted = request.CourseOutlineMilestone[comKey].lessonCompleted;

                        if (request.CourseOutlineMilestoneResourceFile != null)
                        {
                            if (request.CourseOutlineMilestoneResourceFile[comKey].Length > 0)
                            {
                                Stream stream = request.CourseOutlineMilestoneResourceFile[comKey].OpenReadStream();

                                var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                                string courseOutlineMilestoneVideoFolder = String.Format("{0}\\Content\\Video\\CourseOutlineMilestone", path);

                                if (!Directory.Exists(courseOutlineMilestoneVideoFolder))
                                    Directory.CreateDirectory(courseOutlineMilestoneVideoFolder);

                                var milestoneId = Guid.NewGuid();
                                
                                var extension = Path.GetExtension(request.CourseOutlineMilestoneResourceFile[comKey].FileName);
                                var milestoneFileName = milestoneId.ToString() + extension.ToString().ToLower();


                                using (var zipStream = new FileStream(Path.Combine(courseOutlineMilestoneVideoFolder, milestoneFileName), FileMode.Create))
                                {
                                    request.CourseOutlineMilestoneResourceFile[comKey].CopyTo(zipStream);
                                }
                                comModel.resourceFile = milestoneFileName;
                            }
                        }

                        _context.CourseOutlineMilestone.Add(comModel);
                        await _context.SaveChangesAsync();
                    }

                }

                // start: Course Outline Media
                if (request.CourseOutlineMediaFile != null)
                {
                    for (int cofKey = 0; request.CourseOutlineMediaFile.Count() > cofKey; cofKey++)
                    {
                        CourseOutlineMedia cofModel= new CourseOutlineMedia();
                        cofModel.courseOutlineId = model.id;
                        cofModel.courseId = request.courseId;


                        if (request.CourseOutlineMediaFile[cofKey].Length > 0)
                        {
                            Stream stream = request.CourseOutlineMediaFile[cofKey].OpenReadStream();

                            var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                            string courseOutlineMediaFolder = String.Format("{0}\\Content\\Video\\CourseOutlineMedia", path);

                            if (!Directory.Exists(courseOutlineMediaFolder))
                                Directory.CreateDirectory(courseOutlineMediaFolder);

                            var mediaId = Guid.NewGuid();

                            var extension = Path.GetExtension(request.CourseOutlineMediaFile[cofKey].FileName);
                            var mediaFileName = mediaId.ToString() + extension.ToString().ToLower();


                            using (var zipStream = new FileStream(Path.Combine(courseOutlineMediaFolder, mediaFileName), FileMode.Create))
                            {
                                request.CourseOutlineMediaFile[cofKey].CopyTo(zipStream);
                            }
                            cofModel.resourceFile = mediaFileName;
                            }

                        _context.CourseOutlineMedia.Add(cofModel);
                        await _context.SaveChangesAsync();
                    }

                }

                return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully created" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/CourseOutline/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                if (id == 0)
                    return NotFound(_NotFound);

                var response = _courseOutlineRepository.Delete(id);
                if (response == "not exists")
                    return NotFound(_NotFound);
                else if (response == "in used")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course Outline is currently in used." });
                else if (response == "not editable")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
                else
                    return Ok(new GenericResult { Response = true, Message = "Course Outline has been successfully deleted" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpPost("{courseId}/prerequisite/{courseOutlineId}")]
        public IActionResult AddCourseOutlinePrerequisite(long courseId, CourseOutlinePrerequisite request, long courseOutlineId)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canCreate == false)
                    return Unauthorized(_UnAuthorized);

                request.courseOutlineId = courseOutlineId;
                request.courseId = courseId;

                RequiredFields model = new RequiredFields();
                model.CourseOutlinePrerequisite = request;
                object validateFields = _validationService.ValidateRequest("Course Outline Prerequisite", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);


                var output = _courseOutlineRepository.AddPrerequisite(request);
                if (output == true)
                    return Ok(new GenericResult { Response = output, Message = "Prerequisite has been successfully added" });
                else
                    return BadRequest(new GenericResult { Response = output, Message = "Prerequisite already exists" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpDelete("{id}/deleteprerequisite")]
        public IActionResult DeleteCourseOutlinePrerequisite(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseOutlineRepository.DeletePrerequisite(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Course Outline Pre-requisite has been successfully deleted" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }


        [HttpDelete("{id}/media")]
        public IActionResult DeleteCourseOutlineMedia(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseOutlineRepository.DeleteMedia(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Course Outline Media has been successfully deleted" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}/milestone")]
        public IActionResult DeleteCourseOutlineMilestone(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseOutlineRepository.DeleteMilestone(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Course Outline Milestone has been successfully deleted" });
                else
                    return NotFound(new GenericResult { Response = response, Message = "Course Outline Milestone not found" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
