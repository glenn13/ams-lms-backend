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
using lms.Data.Repositories;
using lms.Data.Helpers;
using lms.Data.Services;
using lms.Models;
using Newtonsoft.Json;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAssessmentController : ControllerBase
    { 
        private readonly lmsContext _context;
        private readonly ICourseAssessmentRepository _courseAssessmentRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;

        public CourseAssessmentController(lmsContext context, ICourseAssessmentRepository courseAssessmentRepository, IValidationService validationService)
        {
            _context = context;
            _courseAssessmentRepository = courseAssessmentRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
        }

        // GET: api/CourseAssessment
        [HttpGet("")]
        public IActionResult List()
        {
            try
            {
                var output = _courseAssessmentRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Course assessment record is empty" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{courseId}")]
        public IActionResult ListByCourse(long courseId)
        {
            try
            {
                var output = _courseAssessmentRepository.GetAllByCourse(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Course assessment record is empty" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/CourseAssessment
        [HttpPost]
        public IActionResult Store(CourseAssessment request)
        {

            RequiredFields model = new RequiredFields();
            model.CourseAssessment = request;
            object validateFields = _validationService.ValidateRequest("Course Assessment", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _courseAssessmentRepository.Add(request);

                if (response == false)
                    return BadRequest(new GenericResult { Response = response, Message = "Title, Assessment Type & User Group entry already exists." });
                else
                    return Ok(new GenericResult { Response = true, Message = "Course assessment " + request.title + " has been successfully added" });

            } catch (Exception e) {
                return BadRequest(new GenericResult { Response = false, Message = "Something went wrong" });
            }
        }

        // GET: api/CourseAssessment/5
        [HttpGet("{id}/edit")]
        public IActionResult GetById(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {

                var output = _courseAssessmentRepository.GetById(id);

                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // PUT: api/CourseAssessment/5
        [HttpPut("{id}")]
        public IActionResult Update(long id, CourseAssessment request)
        {

            RequiredFields model = new RequiredFields();
            model.CourseAssessment = request;
            object validateFields = _validationService.ValidateRequest("Course Assessment", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _courseAssessmentRepository.Update(id, request);

                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(new GenericResult { Response = false, Message = request.title + " already exists" });
                else
                    return Ok(new GenericResult { Response = false, Message = request.title + " has been successfully saved" });

            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        // DELETE: api/CourseAssessment/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _courseAssessmentRepository.Delete(id);
                if (response == "not exists")
                    return NotFound(_NotFound);
                else if (response == "in used")
                    return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course Assessment is currently in used." });
                else
                    return Ok(new GenericResult { Response = true, Message = "Course Assessment has been successfully deleted" });


            } catch (Exception e) {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/deleteitem")]
        public IActionResult DeleteItem(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _courseAssessmentRepository.DeleteItem(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record has been successfully deleted" });
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpDelete("{id}/deleteitemchoices")]
        public IActionResult DeleteItemChoices(long id)
        {
            if (id == 0)
                return NotFound(_NotFound);

            try
            {
                var response = _courseAssessmentRepository.DeleteItemChoices(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record has been successfully deleted" });
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
