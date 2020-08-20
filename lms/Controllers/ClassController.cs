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
    public class ClassController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IClassRepository _classRepository;
        private readonly ICourseSessionRepository _courseSessionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IValidationService _validationService; 
        private static object _NotFound;
        private static object _Duplicate;

        public ClassController(lmsContext context, 
                               IClassRepository classRepository,
                               ICourseRepository courseRepository,
                               ICourseSessionRepository courseSessionRepository,
                               ILearnerRepository learnerRepository, 
                               IValidationService validationService)
        {
            _context = context;
            _classRepository = classRepository;
            _courseSessionRepository = courseSessionRepository;
            _courseRepository = courseRepository;
            _learnerRepository = learnerRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _Duplicate = new GenericResult { Response = false, Message = "Record already exists. Cannot enter duplicate entry" };
        }

        [HttpGet("")]
        public IActionResult Index ()
        {
            try
            {
                long userId = 0;
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    userId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Name).Value);
                }

                var output = _classRepository.GetAll(userId);
                    return Ok(output);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }



        [HttpGet("session")]
        public IActionResult InstructorSessions ()
        {
            long userId = 0;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Name).Value);

            try
            {

                var output = _context.CourseSession.Where(x => x.courseInstructorId == userId)
                                                    .Include(x => x.CourseInstructor)
                                                    .ToList();
                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Record is empty" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }




        [HttpGet("learner/{id}/course")]
        public IActionResult CourseLearner (long id)
        {
            try
            {
                var output = _context.Learner.Where(x => x.userId == id)
                                             .Include(x => x.Course.CourseOutline)
                                             .Include(x => x.Course.CourseOutlineMedia)
                                             .Include(x => x.Course.CourseOutlineMilestone)
                                             .Include(x => x.Course.CourseOutlinePrerequisite)
                                             .Include(x => x.User)
                                             .ToList();
                if (output.Count() > 0)
                    return Ok(output);
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet("learner/{userId}/course/{courseId}")]
        public IActionResult CourseLearnerDetails(long userId, long courseId)
        {
            try
            {
                var output = _context.Courses.Where(x => x.id == courseId)
                                             .Include(x => x.CourseOutline)
                                                .ThenInclude(x => x.LearnerCourseOutline.Where(x => x.Learner.userId == userId))
                                                    .ThenInclude(x => x.Status)
                                             .FirstOrDefault();
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

        [HttpGet("learner/{userId}/assessment")]
        public IActionResult CourseAssessment(long userId)
        {
            try
            {
                var output = _learnerRepository.GetLearnerAssessment(userId);
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


        [HttpGet("learner/{userId}/assessment/{courseId}")]
        public IActionResult CourseAssessmentLearnerDetails(long userId, long courseId)
        {
            try
            {

                var output = _context.Learner.Where(x => x.userId == userId && x.courseId == courseId)
                                              .Include(x => x.Course.CourseAssessment)
                                                    .ThenInclude(x => x.AssessmentType)
                                              .Include(x => x.Course.CourseAssessment)
                                                    .ThenInclude(x => x.CourseAssessmentItem)
                                                        .ThenInclude(x => x.CourseAssessmentItemChoices)
                                              .Include(x => x.Course.CourseAssessment)
                                                    .ThenInclude(x => x.CourseAssessmentItem)
                                                        .ThenInclude(x => x.LearnerCourseAssessment)
                                              .FirstOrDefault();
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


        [HttpPost("learner/{learnerId}/assessment/{courseId}/sendReminder")]
        public IActionResult CourseAssessmentLearnerSendReminder(long learnerId, long courseId, LearnerCourseAssessmentReminder request)
        {
            try
            {
                request.learnerId = learnerId;
                request.courseId = courseId;
                RequiredFields model = new RequiredFields();
                model.LearnerCourseAssessmentReminder = request;
                object validateFields = _validationService.ValidateRequest("Learner Course Assessment Reminder", model);
                if (JsonConvert.SerializeObject(validateFields).Length > 2)
                    return BadRequest(validateFields);

                _context.LearnerCourseAssessmentReminder.Add(request);
                _context.SaveChanges();
                return Ok(new GenericResult { Response = true, Message = "Reminder has been set to learner" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPut("learner/{learnerId}/assessment")]
        public IActionResult UpdateLearnerAssessment (long learnerId, Learner request)
        {
            try
            {
                var output = _learnerRepository.AssessLearner(learnerId, request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "Learner assessment completed" });
                else
                    return NotFound(new GenericResult { Response = false, Message = "Learner not found" });
            }
            catch (Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpGet("learner/{userId}/competencies")]
        public IActionResult CourseCompetencies (long userId)
        {
            try
            {
                var output = _learnerRepository.GetLearnerCompetencies(userId);
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

        [HttpGet("learner/{userId}/appraisal")]
        public IActionResult ListAppraisal (long userId)
        {
            try
            {
                var output = _learnerRepository.GetLearnerAppraisal(userId);
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

        [HttpGet("learner/{userId}/appraisal/{courseId}")]
        public IActionResult CourseLearnerAppraisalDetails(long userId, long courseId)
        {
            try
            {
                var output = _context.Learner.Where(x => x.userId == userId && x.courseId == courseId)
                                             .Include(x => x.LearnerAppraisal)
                                                .ThenInclude(x => x.Appraisal)
                                             .FirstOrDefault();
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("learner/{userId}/appraisal")]
        public IActionResult AddUpdateLearnerAppraisal (long userId, Learner request)
        {
            try
            {
                var learner = _context.Learner.Where(x => x.userId == userId && x.courseId == request.courseId).FirstOrDefault();
                if (learner == null)
                    return BadRequest(_NotFound);

                learner.overallRating = request.overallRating;
                _context.SaveChanges();

                var output = _learnerRepository.AddLearnerAppraisal(learner.id, request);

                var hey = JsonConvert.SerializeObject(output);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }   

        [HttpGet("learner/{userId}/attendance")]
        public IActionResult GetLearnerAttendance (long userId)
        {
            var output = _context.Learner.Where(x => x.userId == userId)
                                         .Include(x => x.Course.Session)
                                            .ThenInclude(x => x.LearnerSession)
                                         .Include(x => x.Course.CourseInstructor)
                                            .ThenInclude(x => x.User)
                                         .Include(x => x.User)
                                         .ToList();

            var model = new ArrayList();
            foreach (Learner x in output)
            {
                if (x.Course.Session.Count() > 0)
                {
                    model.Add(x);
                }
            }
            return Ok(model);
        }
    }
}
