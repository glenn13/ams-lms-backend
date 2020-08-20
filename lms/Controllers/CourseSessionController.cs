using System;
using System.Collections;
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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSessionController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseSessionRepository _courseSessionRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;

        public CourseSessionController(lmsContext context, ICourseSessionRepository courseSessionRepository, IValidationService validationService)
        {
            _context = context;
            _courseSessionRepository = courseSessionRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
        }

        // GET: api/CourseSession
        [HttpGet("")]
        public IActionResult List ()
        {
            try
            {
                var output = _courseSessionRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Session Record is empty" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // POST: api/CourseSession
        [HttpPost("")]
        public IActionResult Store(CourseSession request) 
        {

            RequiredFields model = new RequiredFields();
            model.CourseSession = request;
            object validateFields = _validationService.ValidateRequest("Course Session", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                _courseSessionRepository.Add(request);
                return Ok(new GenericResult { Response = true, Message = "Session has been successfully added" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/CourseSession/5
        [HttpGet("{id}")]
        public IActionResult Edit(long id)
        {
            try
            {
                var output = _courseSessionRepository.GetById(id);
                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // PUT: api/CourseSession/5
        [HttpPut("{id}")]

        public IActionResult Update (long id, CourseSession request)
        {
            RequiredFields model = new RequiredFields();
            model.CourseSession = request;
            Object validateFields = _validationService.ValidateRequest("Course Session", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var output = _courseSessionRepository.Update(id, request);
                if (output == true)
                    return Ok(new GenericResult { Response = true, Message = "Session has been successfully updated" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/CourseSession/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var checkLearners = _context.CourseSession.Where(x => x.id == id)
                                                          .Include(x => x.LearnerSession)
                                                          .FirstOrDefault();
                if (checkLearners == null)
                    return NotFound(_NotFound);

                var learnersEnrolled = checkLearners.LearnerSession.Count();

                if (checkLearners.LearnerSession.Count() > 0)
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete! There are (" + learnersEnrolled  + ") confirmed participant(s) for this session" });

                var response = _courseSessionRepository.Delete(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Session has been successfully deleted" });
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }

        }




        [HttpPost("{id}/copy")]
        public IActionResult CopySession(long id, CourseSession request)
        {
            try
            {
                var response = _courseSessionRepository.DuplicateSession(id, request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Session has been copied!" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
