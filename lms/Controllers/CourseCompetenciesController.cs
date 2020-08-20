using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using lms.Data;
using lms.Data.Repositories;
using lms.Data.Helpers;
using lms.Models;
using lms.Data.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using lms.Data.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCompetenciesController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICompetenciesRepository _competenciesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validationService;
        private readonly IFileDirectory _fileDirectory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static object _NotFound;
        private static object _UnAuthorized;

        public CourseCompetenciesController(lmsContext context,
                                            ICompetenciesRepository competenciesRepository,
                                            IUserRepository userRepository,
                                            IValidationService validationService,
                                            IHostingEnvironment hostingEnvironment,
                                            IFileDirectory fileDirectory)
        {
            _context = context;
            _competenciesRepository = competenciesRepository;
            _userRepository = userRepository;
            _validationService = validationService;
            _fileDirectory = fileDirectory;
            _hostingEnvironment = hostingEnvironment;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _UnAuthorized = new GenericResult { Response = false, Message = "You dont have a permission to access this module" };
        }

        // GET: api/Competencies
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            try
            {

                var output = _competenciesRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Competencies Record is empty" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> List(long courseId)
        {
            try
            {

                var output = _competenciesRepository.GetAllByCourse(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Competencies Record is empty" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }


        // POST: api/Competencies
        [HttpPost("")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Store ([FromForm] CourseCompetenciesRequest request)
        {
            try
            {
                RequiredFields model = new RequiredFields();
                CourseCompetencies ccModel = new CourseCompetencies();
                ccModel.courseId = request.courseId;
                ccModel.title = request.title;
                ccModel.description = request.description;
                ccModel.userGroupId = request.userGroupId;
                ccModel.lessonCompleted = request.lessonCompleted;
                ccModel.milestonesReached = request.milestonesReached;
                ccModel.assessmentsSubmitted = request.assessmentsSubmitted;
                ccModel.final = request.final;
                model.Competencies = ccModel;

                object validateFields = _validationService.ValidateRequest("Competencies", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);


                //var response = _competenciesRepository.Add(ccModel);

                var validate = _context.CourseCompetencies.Where(x => x.title == request.title && x.courseId == request.courseId).FirstOrDefault();
                if (validate == null)
                {
                    ccModel.createdAt = DateTime.Now;
                    _context.CourseCompetencies.Add(ccModel);
                    await _context.SaveChangesAsync();

                    var courseCompetenciesId = ccModel.id;
                    if (request.files != null)
                    {

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string competenciesFolder = String.Format("{0}\\Content\\Images\\CourseCompetencies", path);

                        if (!Directory.Exists(competenciesFolder))
                            Directory.CreateDirectory(competenciesFolder);



                        for (int i = 0; i < request.files.Count(); i++)
                        {
                            Stream stream = request.files[i].OpenReadStream();

                            var id = Guid.NewGuid();
                            var extension = Path.GetExtension(request.files[i].FileName);
                            var fileName = id.ToString() + extension.ToString().ToLower();

                            using (var zipStream = new FileStream(Path.Combine(competenciesFolder, fileName), FileMode.Create))
                            {
                                request.files[i].CopyTo(zipStream);
                            }

                            CourseCompetenciesCertificate cccModel = new CourseCompetenciesCertificate();
                            cccModel.courseCompetenciesId = courseCompetenciesId;
                            cccModel.attachment = fileName;
                            _context.CourseCompetenciesCertificate.Add(cccModel);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully added" });
                }
                return BadRequest(new GenericResult { Response = false, Message = request.title + " is already exists" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // GET: api/Competencies/5
        [HttpGet("{id}/edit")]
        public IActionResult Edit (long id)
        {
            if (id == 0)
                return NotFound(_NotFound);
            try
            {
                var output = _competenciesRepository.GetById(id);

                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // PUT: api/Competencies/5
        [HttpPut("{courseCompetenciesId}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update (long courseCompetenciesId, [FromForm] CourseCompetenciesRequest request)
        {
            try
            {

                if (courseCompetenciesId == 0)
                    return NotFound(_NotFound);

                RequiredFields model = new RequiredFields();
                var ccModel = _competenciesRepository.GetById(courseCompetenciesId);
                ccModel.courseId = request.courseId;
                ccModel.title = request.title;
                ccModel.description = request.description;
                ccModel.userGroupId = request.userGroupId;
                ccModel.lessonCompleted = request.lessonCompleted;
                ccModel.milestonesReached = request.milestonesReached;
                ccModel.assessmentsSubmitted = request.assessmentsSubmitted;
                ccModel.final = request.final;
                model.Competencies = ccModel;

                object validateFields = _validationService.ValidateRequest("Competencies", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);

                var checkExists = _context.CourseCompetencies.Where(x => x.id == courseCompetenciesId).Count();
                var validateDuplicate = _context.CourseCompetencies.Where(x => x.title == request.title && x.courseId == request.courseId && x.id != courseCompetenciesId).Count();
                if (validateDuplicate > 0)
                {
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " is already exists" });
                }
                else if (checkExists < 1)
                {
                    return NotFound(_NotFound);
                }
                else
                {
                    //var response = _competenciesRepository.Update(id, request);

                    _context.CourseCompetencies.Update(ccModel);
                    await _context.SaveChangesAsync();

                    if (request.files != null)
                    {

                        var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                        string competenciesFolder = String.Format("{0}\\Content\\Images\\CourseCompetencies", path);

                        if (!Directory.Exists(competenciesFolder))
                            Directory.CreateDirectory(competenciesFolder);


                        for (int i = 0; i < request.files.Count(); i++)
                        {
                            Stream stream = request.files[i].OpenReadStream();

                            var id = Guid.NewGuid();
                            var extension = Path.GetExtension(request.files[i].FileName);
                            var fileName = id.ToString() + extension.ToString().ToLower();

                            using (var zipStream = new FileStream(Path.Combine(competenciesFolder, fileName), FileMode.Create))
                            {
                                request.files[i].CopyTo(zipStream);
                            }

                            CourseCompetenciesCertificate cccModel = new CourseCompetenciesCertificate();
                            cccModel.courseCompetenciesId = courseCompetenciesId;
                            cccModel.attachment = fileName;
                            _context.CourseCompetenciesCertificate.Add(cccModel);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully updated" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        // DELETE: api/Competencies/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _competenciesRepository.Delete(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Competency has been successfully deleted" });
                else
                    return NotFound(_NotFound);


            } catch (Exception e) {
                return BadRequest(e);
            }
        }
    }
}
