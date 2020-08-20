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

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseEvaluationController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly ICourseEvaluationRepository _courseEvaluationRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;
        private static object _Duplicate;

        public CourseEvaluationController(lmsContext context, ICourseEvaluationRepository courseEvaluationRepository, IValidationService validationService)
        {
            _context = context;
            _courseEvaluationRepository = courseEvaluationRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _Duplicate = new GenericResult { Response = false, Message = "Record already exists. Cannot enter duplicate entry" };
        }

        // GET: api/CourseEvaluation
        [HttpGet("")]
        public IActionResult List() 
        {
            try
            {
                var output = _courseEvaluationRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Evaluation Record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/CourseEvaluation/courseid
        [HttpGet("{courseId}")]
        public IActionResult ListByCourse(long courseId) 
        {
            try
            {
                var output = _courseEvaluationRepository.GetAllByCourse(courseId);

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Evaluation Record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/CourseEvaluation
        [HttpPost("")]
        public IActionResult Store (CourseEvaluation request)
        {

            RequiredFields model = new RequiredFields();
            model.CourseEvaluation = request;
            object validateFields = _validationService.ValidateRequest("Course Evaluation", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                //return Ok(request);
                var response = _courseEvaluationRepository.Add(request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = request.title + " has been successfully added" });
                else
                    return BadRequest(_Duplicate);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }


        // GET: api/CourseEvaluation/5
        [HttpGet("{id}/edit")]
        public IActionResult Edit (long id)
        {
            try
            {
                var output = _courseEvaluationRepository.GetById(id);

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

        // PUT: api/CourseEvaluation/5
        [HttpPut("{id}")]
        public IActionResult Update (long id, CourseEvaluation request)
        {

            RequiredFields model = new RequiredFields();
            model.CourseEvaluation = request;
            object validateFields = _validationService.ValidateRequest("Course Evaluation", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {

                var response = _courseEvaluationRepository.Update(id, request);
                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(_Duplicate);
                else
                    return Ok(new GenericResult { Response = true, Message = request.title + " has been successfully updated" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/CourseEvaluation/5
        [HttpDelete("{id}")]
        public IActionResult Delete (long id)
        {
            try
            {
                var response = _courseEvaluationRepository.Delete(id);

                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Evaluation has been successfully deleted" });
                else
                    return NotFound(_NotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpDelete("{id}/values")]
        public IActionResult DeleteValues (long id)
        {
            try
            {
                var response = _courseEvaluationRepository.DeleteValues(id);

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
