using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lms.Data;
using lms.Data.Helpers; 
using lms.Data.Repositories;  
using lms.Data.Services; 
using lms.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections;
using System.Security.Claims;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;

        public EnrollmentController(lmsContext context, 
                                    IEnrollmentRepository enrollmentRepository, 
                                    IInstructorRepository instructorRepository, 
                                    ILearnerRepository learnerRepository, 
                                    IValidationService validationService)
        {
            _context = context;
            _enrollmentRepository = enrollmentRepository;
            _instructorRepository = instructorRepository;
            _learnerRepository = learnerRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
        }

        // GET: api/Enrollment
        [HttpGet("")]
        public async Task<IActionResult> List()
        {  
            try
            {

                long userId = 0;
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    userId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Name).Value);
                }

                var output = _enrollmentRepository.GetAll(userId);

                if (output != null)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Enrollment Record is empty" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }


        [HttpPost("{courseId}")]
        public async Task<IActionResult> EnrollByInstructor(long courseId, VMEnrollment request)
        {
            try
            {
                if (courseId == 0)
                    return BadRequest(new GenericResult { Response = false, Message = "No course selected" });

                var output = _learnerRepository.EnrollByInstructor(courseId, request.sessionId, request);
                if (output > 0)
                    return Ok(new GenericResult { Response = true, Message = "There are (" + output + ") learner enrolled already in course" });
                else
                    return Ok(new GenericResult { Response = true, Message = "Learner successfully enrolled" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // GET: api/Enrollment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var output = _enrollmentRepository.GetById(id);
                if (output != null)
                    return Ok(output); 
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // DELETE: api/Enrollment/5
        [HttpDelete("{learnerId}")]
        public async Task<IActionResult> Delete(long learnerId)
        {
            try
            {
                var response = _learnerRepository.Delete(learnerId);

                if (response == true)
                    return Ok(new GenericResult { Response = true, Message = "Learner has been removed from list of enrollee" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpDelete("{learnerSessionId}/learnersession")]
        public async Task<IActionResult> DeleteLearnerSession(long learnerSessionId)
        {
            try
            {
                var response = _learnerRepository.DeleteLearnerSession(learnerSessionId);
                if (response == false)
                    return NotFound(_NotFound);
                else
                    return Ok(new GenericResult { Response = response, Message = "Session has been successfully deleted" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("session/{id}/learner")]
        public async Task<IActionResult> LearnerEnrollmentSessionList(long id)
        {
            try
            {
                var output = _context.Courses.Include(x => x.Session).ToList();
                return Ok(output);
            } 
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{learnerId}/approve")]
        public async Task<IActionResult> ApproveLearner (long learnerId)
        {
            try
            {
                var output = _learnerRepository.ApproveLearner(learnerId);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "Learner has been sucessfully approved" });
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
