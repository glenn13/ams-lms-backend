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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseInstructorController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;

        public CourseInstructorController(lmsContext context, IInstructorRepository instructorRepository, IValidationService validationService)
        {
            _context = context;
            _instructorRepository = instructorRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
        }

        // GET: api/CourseInstructor
        [HttpGet("")]
        public IActionResult List()
        {
            try
            {
                var output = _instructorRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Instructor Record is empty" });
                 
            } catch (Exception e) {
                return BadRequest(e);
            } 
        }

        // GET: api/CourseInstructorById
        [HttpGet("{courseId}")]
        public IActionResult ListByCourse(long courseId)
        {
            try
            {
                var output = _instructorRepository.GetAllByCourse(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Instructor Record is empty" });
                 
            } catch (Exception e) {
                return BadRequest(e);
            } 
        }

        // POST: api/Instructor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("")]
        public ActionResult Store(CourseInstructor request)
        {

            RequiredFields model = new RequiredFields();
            model.Instructor = request;
            object validateFields = _validationService.ValidateRequest("Instructor", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _instructorRepository.Add(request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Instructor has been successfully saved." });
                else
                    return BadRequest(new GenericResult { Response = response, Message = "Instructor is already assigned in user group." });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // GET: api/Instructor/5
        [HttpGet("{id}/edit")]
        public IActionResult Edit (long id)
        {
            try
            {
                var output = _instructorRepository.GetById(id);
                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // PUT: api/Instructor/5
        [HttpPut("{id}")]
        public IActionResult Update (long id, CourseInstructor request)
        {

            RequiredFields model = new RequiredFields();
            model.Instructor = request;
            object validateFields = _validationService.ValidateRequest("Instructor", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _instructorRepository.Update(id, request);
                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(new GenericResult { Response = false, Message = "Request cannot be process. Instructor already assigned to user group" });
                else
                    return Ok(new GenericResult { Response = true, Message = "Instructor has been updated successfully" });
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // DELETE: api/Instructor/5
        [HttpDelete("{id}")]
        public IActionResult Delete (long id)
        {
            try
            {
                var response = _instructorRepository.Delete(id);
                if (response == "not exists")
                    return NotFound(_NotFound);
                else if (response == "in used")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course Instructor is currently in used." });
                else
                    return Ok(new GenericResult { Response = true, Message = "Course Instructor has been successfully deleted" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        private bool InstructorExists(long id)
        {
            return _context.CourseInstructor.Any(e => e.id == id);
        }
    }
}
