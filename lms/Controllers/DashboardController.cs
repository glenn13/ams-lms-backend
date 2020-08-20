using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using lms.Data;
using lms.Data.Helpers;
using lms.Data.Repositories;
using lms.Data.Services;
using lms.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private static object _NotFound;
        long activeUserId = 0;

        public DashboardController (lmsContext context, IUserRepository userRepository, IAuthenticationService authenticationService)
        {

            _context = context;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
        }

        private object LearnerDashboard(long userId)
        {
            VMModel model = new VMModel();
            model.Courses = _context.Courses.ToList();
            model.Learner = _context.Learner.Include(x => x.User).ToList();
            return model;
        }

        private object AdminDashboard()
        {
            VMModel model = new VMModel();
            model.Courses = _context.Courses.ToList();
            model.Learner = _context.Learner.Include(x => x.User).ToList();
            return model;
        }

        private object InstructorDashboard(long userId)
        {
            VMModel model = new VMModel();
            model.Courses = _context.Courses.ToList();
            model.Learner = _context.Learner.Include(x => x.User).ToList();
            return model;
        }

        // GET: api/<DashboardController>
        [HttpGet("")]
        public IActionResult Dashboard()
        {
            var user = _userRepository.LogCurrentUser();

            if (user.role == "Instructor")
            {
                var output = InstructorDashboard(activeUserId);
                return Ok(output);
            }
            else if (user.role == "Administrator")
            {
                var output = AdminDashboard();
                return Ok(output);
            }
            else
            {
                var output = LearnerDashboard(activeUserId);
                return Ok(output);
            }
        }

        // GET api/<DashboardController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DashboardController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DashboardController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DashboardController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
