using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using lms.Data;
using lms.Models;
using lms.Data.Helpers;
using lms.Data.Repositories;
using lms.Data.Services; 
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using lms.Data.Core;
using System.IO.Compression;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseOutlineRepository _courseOutlineRepository;
        private readonly ICourseOutcomeRepository _courseOutcomeRepository;
        private readonly ICourseAssessmentRepository _courseAssessmentRepository;
        private readonly ICompetenciesRepository _courseCompetenciesRepository;
        private readonly ICourseEvaluationRepository _courseEvaluationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IValidationService _validationService;
        private readonly IEncryptionService _encryptionService;
        private readonly IFileDirectory _fileDirectory;
        private static object _NotFound;
        private static object _UnAuthorized;

        public CoursesController(lmsContext context, 
                                 ICourseRepository courseRepository,
                                 ICourseOutlineRepository courseOutlineRepository,
                                 ICourseOutcomeRepository courseOutcomeRepository,
                                 ICourseAssessmentRepository courseAssessmentRepository,
                                 ICompetenciesRepository courseCompetenciesRepository,
                                 ICourseEvaluationRepository courseEvaluationRepository,
                                 IUserRepository userRepository,
                                 IValidationService validationService, 
                                 IEncryptionService encryptionService,
                                 IFileDirectory fileDirectory,
                                 IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _courseRepository = courseRepository;
            _courseOutlineRepository = courseOutlineRepository;
            _courseAssessmentRepository = courseAssessmentRepository;
            _courseCompetenciesRepository = courseCompetenciesRepository;
            _courseEvaluationRepository = courseEvaluationRepository;
            _courseOutcomeRepository = courseOutcomeRepository;
            _userRepository = userRepository;
            _validationService = validationService;
            _encryptionService = encryptionService;
            _fileDirectory = fileDirectory;
            _hostingEnvironment = hostingEnvironment;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _UnAuthorized = new GenericResult { Response = false, Message = "You dont have a permission to access this module" };
        }

        // GET: api/Courses
        [HttpGet("")]
        public IActionResult List()
        {
            try
            {
                var output = _courseRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Course record is empty" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }


        // POST: api/Courses
        [HttpPost("")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Store([FromForm] CourseRequest request)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canCreate == false)
                    return Unauthorized(_UnAuthorized);

                Courses model = new Courses();
                model.title =  request.title;
                model.description = request.description;
                model.status = request.status;
                model.isPublished = request.isPublished;
                model.durationTime = request.durationTime;
                model.durationType = request.durationType;
                model.passingGrade = request.passingGrade;
                model.capacity = request.capacity;


                RequiredFields validation = new RequiredFields();
                validation.Courses = model;
                object validateFields = _validationService.ValidateRequest("Courses", validation);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);

                var checkExists =_context.Courses.Where(x => x.title == request.title).Any();
                if (checkExists == true)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " is already exists" });

                var refCode = _encryptionService.GenerateRefCode();
                model.code = refCode;
                model.CourseLevel = request.CourseLevel;
                model.CourseCategory = request.CourseCategory;
                model.CourseType = request.CourseType;
                model.CourseLanguage = request.CourseLanguage;
                model.CourseTag = request.CourseTag;
                model.createdAt = DateTime.Now;

                if (request.featureImage != null)
                {
                    if (request.featureImage.Length > 0)
                    {
                        Stream stream = request.featureImage.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseImageFolder = String.Format("{0}\\Content\\Images\\Course", path);

                        if (!Directory.Exists(courseImageFolder))
                            Directory.CreateDirectory(courseImageFolder);

                        var id = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureImage.FileName);
                        var fileName = id.ToString() + extension.ToString().ToLower();


                        using (var zipStream = new FileStream(Path.Combine(courseImageFolder, fileName), FileMode.Create, FileAccess.Write))
                        {
                            request.featureImage.CopyTo(zipStream);
                        }
                        model.featureImage = fileName;

                    }
                }

                if (request.featureVideo != null)
                {
                    if (request.featureVideo.Length > 0)
                    {
                        Stream stream = request.featureVideo.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseVideoFolder = String.Format("{0}\\Content\\Video\\Course", path);

                        if (!Directory.Exists(courseVideoFolder))
                            Directory.CreateDirectory(courseVideoFolder);

                        var id = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureVideo.FileName);
                        var fileName = id.ToString() + extension.ToString().ToLower();


                        using (var zipStream = new FileStream(Path.Combine(courseVideoFolder, fileName), FileMode.Create, FileAccess.Write))
                        {
                            request.featureVideo.CopyTo(zipStream);
                        }
                        model.featureVideo = fileName;

                    }
                }

                _context.Courses.Add(model);
                await _context.SaveChangesAsync();

                for (int i = 0; request.RelatedCourse.Count() > i; i++)
                {
                    CourseRelatedDetails crdModel = new CourseRelatedDetails();
                    crdModel.courseId = model.id;
                    crdModel.isPrerequisite = request.RelatedCourse[i].isPrerequisite;
                    _context.CourseRelated.Add(crdModel);
                    await _context.SaveChangesAsync();

                    CourseRelatedList crlModel = new CourseRelatedList();
                    crlModel.courseRelatedId = crdModel.id;
                    crlModel.courseId = request.RelatedCourse[i].relatedCourseId;
                    _context.CourseRelatedList.Add(crlModel);
                    await _context.SaveChangesAsync();
                }

                return Ok(new GenericResult { Response = true, Message = "Course " + request.title + " successfully created" });


            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public IActionResult Edit(long id)
        {
            try
            {
                if (id == 0)
                    return NotFound(_NotFound);

                var output = _courseRepository.GetById(id);

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




        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update(long id, [FromForm] CourseRequest request)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canModify == false)
                    return Unauthorized(_UnAuthorized);

                var checkExists = _context.Courses.Where(x => x.title == request.title && x.id != id).Any();
                if (checkExists == true)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " is already exists" });

                var model = _context.Courses.Where(x => x.id == id).FirstOrDefault();

                if (model == null || id == 0)
                    return NotFound(_NotFound);

                    model.title = request.title;
                    model.description = request.description;
                    model.durationTime = request.durationTime;
                    model.durationType = request.durationType;
                    model.passingGrade = request.passingGrade;
                    model.capacity = request.capacity;

                    RequiredFields modelRequried = new RequiredFields();
                    modelRequried.Courses = model;
                    object validateFields = _validationService.ValidateRequest("Courses", modelRequried);
                    if (JsonConvert.SerializeObject(validateFields).Length > 2)
                        return BadRequest(validateFields);

                model.CourseLevel = request.CourseLevel;
                model.CourseCategory = request.CourseCategory;
                model.CourseType = request.CourseType;
                model.CourseLanguage = request.CourseLanguage;
                model.CourseTag = request.CourseTag;

                //var response = _courseRepository.Update(id, request);

                if (request.featureImage != null)
                {
                    if (request.featureImage.Length > 0)
                    {
                        Stream stream = request.featureImage.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseImageFolder = String.Format("{0}\\Content\\Images\\Course", path);

                        if (!Directory.Exists(courseImageFolder))
                            Directory.CreateDirectory(courseImageFolder);

                        var fileId = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureImage.FileName);
                        var fileName = fileId.ToString() + extension.ToString().ToLower();

                        if (System.IO.File.Exists(Path.Combine(courseImageFolder, model.featureImage)))
                        {
                            System.IO.File.Delete(Path.Combine(courseImageFolder, model.featureImage));
                        }

                        using (var zipStream = new FileStream(Path.Combine(courseImageFolder, fileName), FileMode.Create, FileAccess.Write))
                        {
                            request.featureImage.CopyTo(zipStream);
                        }

                        model.featureImage = fileName;

                    }
                }

                if (request.featureVideo != null)
                {
                    if (request.featureVideo.Length > 0)
                    {
                        Stream stream = request.featureVideo.OpenReadStream();

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string courseVideoFolder = String.Format("{0}\\Content\\Video\\Course", path);

                        if (!Directory.Exists(courseVideoFolder))
                            Directory.CreateDirectory(courseVideoFolder);

                        var fileId = Guid.NewGuid();
                        var extension = Path.GetExtension(request.featureVideo.FileName);
                        var fileName = fileId.ToString() + extension.ToString().ToLower();

                        if (System.IO.File.Exists(Path.Combine(courseVideoFolder, model.featureVideo)))
                        {
                            System.IO.File.Delete(Path.Combine(courseVideoFolder, model.featureVideo));
                        }

                        using (var zipStream = new FileStream(Path.Combine(courseVideoFolder, fileName), FileMode.Create, FileAccess.Write))
                        {
                            request.featureVideo.CopyTo(zipStream);
                        }

                        model.featureVideo = fileName;

                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully updated" });

            } 
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                if (id == 0)
                    return NotFound(_NotFound);

                var response = _courseRepository.Delete(id);
                if (response == "not exists")
                    return NotFound(_NotFound);
                else if (response == "in used")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course is currently in used." });
                else if (response == "not editable")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
                else
                    return Ok(new GenericResult { Response = true, Message = "Course has been successfully deleted" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }



        [HttpPost("requestpublish")]
        public IActionResult RequestPublishCourse(PublishCourse request)
        {
            try
            {
                var response = _courseRepository.RequestPublishCourse(request);

                if (response == true)
                {
                    return Ok(new GenericResult { Response = response, Message =  "Publish request sent." });
                }
                else
                {
                    return NotFound(_NotFound);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("publish")]
        public IActionResult PublishCourse(PublishCourse request)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canModify == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.PublishCourse(request);

                if (response == true)
                {
                    return Ok(new GenericResult { Response = response, Message = "Course has been successfully published." });
                }
                else
                {
                    return NotFound(_NotFound);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("unpublish")]
        public IActionResult UnpublishCourse(PublishCourse request)
        {

            try
            {
            if (_userRepository.LogCurrentUser().canModify == false)
                return Unauthorized(_UnAuthorized);

                var response = _courseRepository.UnpublishCourse(request);

                if (response == true)
                {
                    var courses = _context.Courses.Where(x => x.id == request.courseId).FirstOrDefault();
                    return Ok(new GenericResult { Response = response, Message = "Course " + courses.title + " has been successfully unpublished." });
                }
                else
                {
                    return NotFound(_NotFound);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}/courselevel")]
        public IActionResult DeleteCourseLevel(long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCourseLevel(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected level has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/coursecategory")]
        public IActionResult DeleteCourseCategory (long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCourseCategory(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected category has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/coursetype")]
        public IActionResult DeleteCourseType (long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCourseType(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected course type has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/courserelated")]
        public IActionResult DeleteCourseRelated (long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCourseRelated(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected related course has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/courselanguage")]
        public IActionResult DeleteCourseLanguage (long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCourseLanguage(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected language has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/coursetags")]
        public IActionResult DeleteCouseTag (long id)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canRemove == false)
                    return Unauthorized(_UnAuthorized);

                var response = _courseRepository.DeleteCouseTag(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Selected tag has been successfully removed from the course" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

     

        [HttpPost("{id}/copy")]
        public IActionResult CopyCourse(long id, Courses request)
        {
            try
            {
                if (_userRepository.LogCurrentUser().canCreate == false)
                    return Unauthorized(_UnAuthorized);

                if (id == 0)
                    return BadRequest(new RequestValidationModel { Name = "Course", Parameter = "courseId", Message = "Please select course" });
                else if (String.IsNullOrEmpty(request.title) == true)
                    return BadRequest(new RequestValidationModel { Name = "New Course Title", Parameter = "title", Message = "Please enter new course title" });

                var model = _courseRepository.GetByIdCourseSimple(id);
                var model2 = _courseRepository.GetByName(request.title);
                if (model == null)
                    return NotFound(_NotFound);

                if (model2 != null)
                    return BadRequest(new GenericResult { Response = false, Message = "Course already exists" });


                var refCode = _encryptionService.GenerateRefCode();


                _courseRepository.DuplicateCourse(id, refCode, request.title);


                var insertedCourse = _courseRepository.GetByCode(refCode);

                _courseRepository.GetByCode(refCode);


                _courseOutlineRepository.DuplicateByCourseId(id, insertedCourse.id);

                _courseOutcomeRepository.DuplicateByCourseId(id, insertedCourse.id);

                _courseAssessmentRepository.DuplicateByCourseId(id, insertedCourse.id);

                _courseCompetenciesRepository.DuplicateByCourseId(id, insertedCourse.id);

                _courseEvaluationRepository.DuplicateByCourseId(id, insertedCourse.id);


                return Ok(new GenericResult { Response = true, Message = "Course has been successfully copied" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

    }
}
