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

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly lmsContext _context;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IValidationService _validationService;
        private static object _NotFound;
        private static object _Duplicate;

        public SettingsController(lmsContext context, 
                                  IUserGroupRepository userGroupRepository, 
                                  IGroupRepository groupRepository, 
                                  IStatusRepository statusRepository, 
                                  IValidationService validationService)
        {
            _context = context;
            _userGroupRepository = userGroupRepository;
            _groupRepository = groupRepository;
            _statusRepository = statusRepository;
            _validationService = validationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _Duplicate = new GenericResult { Response = false, Message = "Record already exists. Cannot enter duplicate entry" };
        }
        #region USER GROUPS

        [HttpGet("usergroup")]
        public IActionResult UserGroupList()
        {
            try
            {

                var output = _userGroupRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "User group record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        } 
         
        [HttpPost("usergroup")]
        public IActionResult UserGroupStore (UserGroups request)
        {
            RequiredFields model = new RequiredFields();
            model.UserGroups = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("User Group", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _userGroupRepository.Add(request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });
                else
                    return BadRequest(_Duplicate);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("usergroup/{id}")]
        public IActionResult UserGroupEdit (long id)
        {
            try
            {
                var output = _userGroupRepository.GetById(id);
                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpPut("usergroup/{id}")]
        public IActionResult UserGroupUpdate (long id, UserGroups request)
        {
            RequiredFields model = new RequiredFields();
            model.UserGroups = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("User Group", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _userGroupRepository.Update(id, request);
                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(_Duplicate);
                else
                    return Ok(new GenericResult { Response = true, Message = request.name + " successfully updated" });
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpDelete("usergroup/{id}")]
        public IActionResult UserGroupDelete (long id)
        {
            try
            {
                var response = _userGroupRepository.Delete(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record successfully deleted" });
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        #endregion USER GROUPS


        #region GROUPS

        [HttpGet("group")]
        public IActionResult GroupList()
        {
            try
            {

                var output = _groupRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Group record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost("group")]
        public IActionResult GroupStore(Groups request)
        {
            RequiredFields model = new RequiredFields();
            model.Groups = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("Groups", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _groupRepository.Add(request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record has been successfully added" });
                else
                    return BadRequest(_Duplicate);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("group/{id}")]
        public IActionResult GroupEdit(long id)
        {
            try
            {
                var output = _groupRepository.GetById(id);
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

        [HttpPut("group/{id}")]
        public IActionResult GroupUpdate(long id, Groups request)
        {
            RequiredFields model = new RequiredFields();
            model.Groups = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("Groups", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _groupRepository.Update(id, request);
                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(_Duplicate);
                else
                    return Ok(new GenericResult { Response = true, Message = "Record successfully updated" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("group/{id}")]
        public IActionResult GroupDelete(long id)
        {
            try
            {
                var response = _groupRepository.Delete(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record successfully deleted" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("group/{id}/searchbyusergroup")]
        public IActionResult SearchByUserGroup (long id)
        {
            try
            {
                var output = _groupRepository.SearchByUserGroup(id);
                if (output != null)
                    return Ok(output);
                else
                    return NotFound(_NotFound);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }
        #endregion GROUPS


        #region STATUS

        [HttpGet("status")]
        public IActionResult StatusList()
        {
            try
            {

                var output = _statusRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = true, Message = "Status record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost("status")]
        public IActionResult StatusStore(Status request)
        {
            RequiredFields model = new RequiredFields();
            model.Status = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("Status", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _statusRepository.Add(request);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record has been successfully added" });
                else
                    return BadRequest(_Duplicate);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("status/{id}")]
        public IActionResult StatusEdit(long id)
        {
            try
            {
                var output = _statusRepository.GetById(id);
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

        [HttpPut("status/{id}")]
        public IActionResult StatusUpdate(long id, Status request)
        {
            RequiredFields model = new RequiredFields();
            model.Status = request;
            //return Ok(model);
            object validateFields = _validationService.ValidateRequest("Status", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _statusRepository.Update(id, request);
                if (response == 0)
                    return NotFound(_NotFound);
                else if (response == 1)
                    return BadRequest(_Duplicate);
                else
                    return Ok(new GenericResult { Response = true, Message = "Record successfully updated" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("status/{id}")]
        public IActionResult StatusDelete(long id)
        {
            try
            {
                var response = _statusRepository.Delete(id);
                if (response == true)
                    return Ok(new GenericResult { Response = response, Message = "Record successfully deleted" });
                else
                    return NotFound(_NotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion STATUS



        //// GET: api/Settings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Groups>>> GetGroups()
        //{
        //    return await _context.Groups.ToListAsync();
        //}

        //// GET: api/Settings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Groups>> GetGroups(long id)
        //{
        //    var groups = await _context.Groups.FindAsync(id);

        //    if (groups == null)
        //    {
        //        return NotFound();
        //    }

        //    return groups;
        //}

        //// PUT: api/Settings/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGroups(long id, Groups groups)
        //{
        //    if (id != groups.id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(groups).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GroupsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Settings
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Groups>> PostGroups(Groups groups)
        //{
        //    _context.Groups.Add(groups);
        //    await _context.SaveChangesAsync(); 

        //    return CreatedAtAction("GetGroups", new { id = groups.id }, groups);
        //}

        //// DELETE: api/Settings/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGroups(long id)
        //{
        //    var groups = await _context.Groups.FindAsync(id);
        //    if (groups == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Groups.Remove(groups);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool GroupsExists(long id)
        //{
        //    return _context.Groups.Any(e => e.id == id);
        //}
    }
}
