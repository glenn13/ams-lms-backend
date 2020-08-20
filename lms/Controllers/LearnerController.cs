using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lms.Data;
using lms.Models;
using lms.Data.Repositories;
using lms.Data.Helpers;
using lms.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Claims;


namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;
        private static object _Duplicate;

        public LearnerController (lmsContext context, ICourseRepository courseRepository, ILearnerRepository learnerRepository, IValidationService validationService)
        {
            _context = context;
            _courseRepository = courseRepository;
            _learnerRepository = learnerRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _Duplicate = new GenericResult { Response = false, Message = "Record already exists. Cannot enter duplicate entry" };
        }


        // GET: api/<LearnerController>
        [HttpGet]
        public IActionResult index()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{userId}/MyCourse")]
        public IActionResult LearnerCourse (long userId)
        {
            try
            {
                var output = _learnerRepository.LearnerCourse(userId);
                if (output != null)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Learner doesn't enrolled to any course yet" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // @ SELF ENROLL | PROCESSED BY LEARNER
        // GET api/<LearnerController>/5/1
        [HttpPost("enrollment/{courseId}/{sessionId}")]
        public async Task<IActionResult> EnrollByLearner(long courseId, long sessionId)
        {
            try
            {

                if (courseId == 0)
                    return BadRequest(new GenericResult { Response = false, Message = "No course selected" });


                long userId = 0;
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    userId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Name).Value);
                }

                var enrollmentStatus = _context.Status.Where(x => x.category == "Enrollment" && x.name == "New").FirstOrDefault();

                Learner request = new Learner();
                request.courseId = courseId;
                request.userId = userId;
                request.enrollmentType = 0; // self enrolled
                request.isNotify = 0;
                request.statusId = enrollmentStatus.id;
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                _context.Learner.Add(request);
                await _context.SaveChangesAsync();

                return Ok(request);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // PUT: api/Learner/5/updateEnrollment
        [HttpPut("{learnerId}/updateenrollment/{sessionId}")]
        public IActionResult UpdateLeanerEnrollment(long learnerId, long sessionId, Learner request)
        {
            try
            {
                var output = _learnerRepository.UpdateLeanerEnrollment(learnerId, sessionId, request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "Learner has been successfully updated" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost("courseoutline")]
        public IActionResult AddLearnerOutline(LearnerCourseOutline request)
        {
            try
            {
                RequiredFields model = new RequiredFields();
                model.LearnerCourseOutline = request;
                object validateFields = _validationService.ValidateRequest("Learner Course Outline", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);


                var output = _learnerRepository.AddCourseOutline(request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "You have successfully completed the course" });
                else
                    return BadRequest(new GenericResult { Response = false, Message = "Course lesson already taken" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost("courseassessment")]
        public IActionResult AddLearnerAssessment(LearnerCourseAssessment request)
        {
            try
            {
                RequiredFields model = new RequiredFields();
                model.LearnerCourseAssessment = request;
                object validateFields = _validationService.ValidateRequest("Learner Course Assessment", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);


                var output = _learnerRepository.AddCourseAssessment(request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "you have successfully completed the questionnaires" });
                else
                    return BadRequest(new GenericResult { Response = false, Message = "You have reached the available maximum attempts" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("{learnerId}/CourseReview")]
        public IActionResult CourseReview(long learnerId, Learner request)
        {
            try
            {
                var output = _learnerRepository.AddCourseReview(learnerId, request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "Review posted" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
