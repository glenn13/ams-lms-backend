using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lms.Models;
using lms.Data.Services;
using lms.Data.Helpers;
using Newtonsoft.Json.Schema; 

namespace lms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        public AuthController (IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult post ([FromBody] Users model)
        {
            var user = _authenticationService.Authenticate(model.username, model.password);

            if (user == null)
                return BadRequest(new GenericResult{ Response = false, Message = "username or password is incorrect"});

            return Ok(user);
        }
    }
}
