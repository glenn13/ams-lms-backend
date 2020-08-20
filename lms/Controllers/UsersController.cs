using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lms.Data;
using lms.Data.Repositories;
using lms.Models;
using lms.Data.Helpers;
using lms.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IValidationService _validationService;
        private static object _NotFound;
        private static object _UnAuthorized;

        public UsersController(lmsContext context, IUserRepository userRepository, IEncryptionService encryptionService, IValidationService validationService)
        {
            _context = context;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _UnAuthorized = new GenericResult { Response = false, Message = "You dont have a permission to access this module" };
        }

        // GET: api/Users
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUsersAsync(Users request)
        {
            try
            {
                var validations = _validationService.registrationValidation(request);
                var validationPassed = validations.GetType().GetProperties().First(x => x.Name == "Response").GetValue(validations, null);
                if (validationPassed.ToString().ToLower() == "false")
                    return BadRequest(validations);

                var output = _userRepository.Add(request);

                if (output != null)
                    return Ok(new GenericResult { Response = true, Message = "User Created" });
                else
                    return BadRequest(new GenericResult { Response = false, Message = "Something went wrong in creating user" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("selection")]
        public async Task<IActionResult> UserSelection ()
        {
            try
            {
                var output = _userRepository.UserSelection();
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Users request)
        {
            try
            {
                var output = _userRepository.UpdateAsync(id, request);

                if (output == "success")
                    return NotFound(new GenericResult { Response = true, Message = "User information updated" });
                else
                    return NotFound(new GenericResult { Response = false, Message = output });

                return Ok(request);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }


            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!UsersExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(long id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(long id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
