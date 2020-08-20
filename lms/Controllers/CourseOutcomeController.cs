using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using lms.Data;
using lms.Models;
using lms.Data.Repositories; 
using lms.Data.Helpers;
using lms.Data.Services;
using Newtonsoft.Json;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOutcomeController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseOutcomeRepository _courseOutcomeRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;

        public CourseOutcomeController(lmsContext context, ICourseOutcomeRepository courseOutcomeRepository, IValidationService validationService)
        {
            _context = context;
            _courseOutcomeRepository = courseOutcomeRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };

        }

        // GET: api/CourseOutcome
        [HttpGet("")]
        public IActionResult List()
        {
            try
            {
                var output = _courseOutcomeRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Learning Outcome record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/CourseOutcome
        [HttpGet("{courseId}")]
        public IActionResult ListByCourse(long courseId)
        {
            try
            {
                var output = _courseOutcomeRepository.GetAllByCourseId(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Learning Outcome record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/CourseOutcome/5/edit
        [HttpGet("{id}/edit")]
        public IActionResult Edit(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var output = _courseOutcomeRepository.GetById(id);
                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // PUT: api/CourseOutcome/5
        [HttpPut("{id}")]
        public IActionResult Update(long id, CourseOutcome request)
        {
            if (id == 0)
                return NotFound(_NotFound);

            RequiredFields model = new RequiredFields();
            model.CourseOutcome = request;
            object validateFields = _validationService.ValidateRequest("Course Outcome", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _courseOutcomeRepository.Update(id, request);

                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " already exists" });
                else
                    return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully updated" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // POST: api/CourseOutcome
        [HttpPost("")]
        public IActionResult Store(CourseOutcome request)
        {

            RequiredFields model = new RequiredFields();
            model.CourseOutcome = request;
            object validateFields = _validationService.ValidateRequest("Course Outcome", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _courseOutcomeRepository.Add(request);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = request.title + " has been sucessfully added" });
                else
                    return BadRequest(new GenericResult { Response = response, Message = request.title + " already exists" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // DELETE: api/CourseOutcome/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _courseOutcomeRepository.Delete(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Course Outcome has been successfully deleted"});
                else
                    return NotFound(_NotFound);


            } catch (Exception e) {
                return BadRequest(e);
            };
        }

    }
}
