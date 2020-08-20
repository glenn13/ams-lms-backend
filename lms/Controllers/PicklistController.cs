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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
//using lms.Data.Repositories.Abstract;
//using lms.Data.Repositories.Abstract;

namespace lms.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PicklistController : ControllerBase
    {
        private readonly lmsContext _context;
        //private readonly ISessionTypeRepository _sessionTypeRepository;
        //private readonly ITagsRepository _tagsRepository;
        //private readonly ILocationRepository _locationRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        //private readonly ICategoryRepository _categoryRepository;
        private readonly IAppraisalRepository _appraisalRepository;
        //private readonly ILanguageRepository _languageRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly ILevelRepository _levelRepository;
        //private readonly ITypesRepository _typesRepository;
        private readonly IValidationService _validationService;
        private readonly IAuthenticationService _authenticationService;
        private static object _NotFound;
        private static object _Duplicate;
        private static object _UnAuthorized;

        public PicklistController(
                                  lmsContext context,
                                  //ISessionTypeRepository sessionTypeRepository, 
                                  //ITagsRepository tagsRepository,
                                  //ILocationRepository locationRepository,
                                  //IDepartmentRepository departmentRepository,
                                  //ICategoryRepository categoryRepository,
                                  IAppraisalRepository appraisalRepository,
                                  //ILanguageRepository languageRepository,
                                  //IUserRepository userRepository,
                                  //ILevelRepository levelRepository,
                                  //ITypesRepository typesRepository,
                                  IValidationService validationService,
                                  IAuthenticationService authenticationService
            )
        {
            _context = context;
            //_sessionTypeRepository = sessionTypeRepository;
            //_tagsRepository = tagsRepository;
            //_locationRepository = locationRepository;
            //_departmentRepository = departmentRepository;
            //_categoryRepository = categoryRepository;
            _appraisalRepository = appraisalRepository;
            //_languageRepository = languageRepository;
            //_userRepository = userRepository;
            //_levelRepository = levelRepository;
            //_typesRepository = typesRepository;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _NotFound = new GenericResult { Response = false, Message = "Record not found" };
            _Duplicate = new GenericResult { Response = false, Message = "Record already exists. Cannot enter duplicate entry" };
            _UnAuthorized = new GenericResult { Response = false, Message = "You dont have a permission to access this module" };
        }


        #region Appraisal
        // GET: api/Picklist/Appraisal
        [HttpGet("appraisal")]
        public async Task<IActionResult> AppraisalList()
        {
            try
            {
                var output = _appraisalRepository.GetAll();

                if (output.Count() > 0)
                    return Ok(output);
                else
                    return Ok(new GenericResult { Response = false, Message = "Appraisal record is empty" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //// POST: api/Picklist/Appraisal
        [HttpPost("appraisal")]
        public async Task<IActionResult> AppraisalStore(Appraisal request)
        {
            //if (_userRepository.LogCurrentUser().role != "Administrator")
            //    return Unauthorized(_UnAuthorized);

            RequiredFields model = new RequiredFields();
            model.Appraisal = request;
            object validateFields = _validationService.ValidateRequest("Appraisal", model);
            if (JsonConvert.SerializeObject(validateFields).Length > 2)
                return BadRequest(validateFields);

            try
            {
                var response = _appraisalRepository.Add(request);
                if (response == false)
                    return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
                else
                    return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //// GET: api/Picklist/Appraisal/5
        //[HttpGet("appraisal/{id}")]
        //public IActionResult AppraisalEdit(long id)
        //{
        //    try
        //    {
        //        var output = _appraisalRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("appraisal/{id}")]
        //public IActionResult AppraisalUpdate(long id, Appraisal request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Appraisal = request;
        //    object validateFields = _validationService.ValidateRequest("Appraisal", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _appraisalRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("appraisal/{id}")]
        //public IActionResult AppraisalDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _appraisalRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Appraisal is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Appraisal has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        #endregion Appraisal


        //#region Session Type
        //// GET: api/Picklist/sessiontype
        //[HttpGet("sessiontype")]
        //public IActionResult SessionTypeList()
        //{

        //    try
        //    {
        //        var output = _sessionTypeRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Session type record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/sessiontype
        //[HttpPost("sessiontype")]
        //public IActionResult SessionTypeStore(SessionType request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.SessionType = request;
        //    object validateFields = _validationService.ValidateRequest("Session Type", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _sessionTypeRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    } catch (Exception e) {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/sessiontype/5
        //[HttpGet("sessiontype/{id}")]
        //public IActionResult SessionTypeEdit(long id)
        //{
        //    if (id == 0)
        //        return NotFound(_NotFound);

        //    try
        //    {
        //        var output = _sessionTypeRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    } catch (Exception e) {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("sessiontype/{id}")]
        //public IActionResult SessionTypeUpdate(long id, SessionType request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.SessionType = request;
        //    object validateFields = _validationService.ValidateRequest("Session Type", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _sessionTypeRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    } catch (Exception e) {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("sessiontype/{id}")]
        //public IActionResult SessionTypeDelete (long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _sessionTypeRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Session type is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Session type has been successfully deleted" });

        //    } catch (Exception e) {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Session Type


        //#region Tags
        //// GET: api/Picklist/tag
        //[HttpGet("tag")]
        //public IActionResult TagsList()
        //{
        //    try
        //    {
        //        var output = _tagsRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Tag record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/tag
        //[HttpPost("tag")]
        //public IActionResult TagStore(Tags request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Tags = request;
        //    object validateFields = _validationService.ValidateRequest("Tags", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _tagsRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/tag/5
        //[HttpGet("tag/{id}")]
        //public IActionResult TagEdit(long id)
        //{
        //    try
        //    {
        //        var output = _tagsRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("tag/{id}")]
        //public IActionResult TagUpdate(long id, Tags request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Tags = request;
        //    object validateFields = _validationService.ValidateRequest("Tags", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _tagsRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("tag/{id}")]
        //public IActionResult TagDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _tagsRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Tag is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Tag has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Tags


        //#region Locations
        //// GET: api/Picklist/location
        //[HttpGet("location")]
        //public IActionResult LocationList()
        //{
        //    try
        //    {
        //        var output = _locationRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Location record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/location
        //[HttpPost("location")]
        //public IActionResult LocationStore(Location request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Location = request;
        //    object validateFields = _validationService.ValidateRequest("Location", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _locationRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/tag/5
        //[HttpGet("location/{id}")]
        //public IActionResult LocationEdit(long id)
        //{
        //    try
        //    {
        //        var output = _locationRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("location/{id}")]
        //public IActionResult LocationUpdate(long id, Location request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Location = request;
        //    object validateFields = _validationService.ValidateRequest("Location", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _locationRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("location/{id}")]
        //public IActionResult LocationDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _locationRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Location has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Locations


        //#region Department
        //// GET: api/Picklist/department
        //[HttpGet("department")]
        //public IActionResult DepartmentList()
        //{
        //    try
        //    {
        //        var output = _departmentRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Department record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/department
        //[HttpPost("department")]
        //public IActionResult DepartmentStore(Department request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Department = request;
        //    object validateFields = _validationService.ValidateRequest("Department", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _departmentRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/department/5
        //[HttpGet("department/{id}")]
        //public IActionResult DepartmentEdit(long id)
        //{
        //    try
        //    {
        //        var output = _departmentRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("department/{id}")]
        //public IActionResult DepartmentUpdate(long id, Department request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Department = request;
        //    object validateFields = _validationService.ValidateRequest("Department", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _departmentRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("department/{id}")]
        //public IActionResult DepartmentDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _departmentRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Department is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Department has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Department


        //#region Course Category
        //// GET: api/Picklist/Category
        //[HttpGet("category")]
        //public IActionResult CategoryList()
        //{
        //    try
        //    {
        //        var output = _categoryRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Course Category record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/Category
        //[HttpPost("category")]
        //public IActionResult CategoryStore(Category request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Category = request;
        //    object validateFields = _validationService.ValidateRequest("Category", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _categoryRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/Category/5
        //[HttpGet("category/{id}")]
        //public IActionResult CategoryEdit(long id)
        //{
        //    try
        //    {
        //        var output = _categoryRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("category/{id}")]
        //public IActionResult CategoryUpdate(long id, Category request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Category = request;
        //    object validateFields = _validationService.ValidateRequest("Category", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _categoryRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("category/{id}")]
        //public IActionResult CategoryDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _categoryRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Category is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Category has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Course Category


        //#region Appraisal
        //// GET: api/Picklist/Appraisal
        //[HttpGet("appraisal")]
        //public async Task<IActionResult> AppraisalList()
        //{
        //    try
        //    {
        //        var output = _appraisalRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Appraisal record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/Appraisal
        //[HttpPost("appraisal")]
        //public IActionResult AppraisalStore(Appraisal request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Appraisal = request;
        //    object validateFields = _validationService.ValidateRequest("Appraisal", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _appraisalRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/Appraisal/5
        //[HttpGet("appraisal/{id}")]
        //public IActionResult AppraisalEdit(long id)
        //{
        //    try
        //    {
        //        var output = _appraisalRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("appraisal/{id}")]
        //public IActionResult AppraisalUpdate(long id, Appraisal request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Appraisal = request;
        //    object validateFields = _validationService.ValidateRequest("Appraisal", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _appraisalRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("appraisal/{id}")]
        //public IActionResult AppraisalDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _appraisalRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Appraisal is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Appraisal has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Appraisal


        //#region Language
        //// GET: api/Picklist/Language
        //[HttpGet("language")]
        //public IActionResult LanguageList()
        //{
        //    try
        //    {
        //        var output = _languageRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Language record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/Language
        //[HttpPost("language")]
        //public IActionResult LanguageStore(Language request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Language = request;
        //    object validateFields = _validationService.ValidateRequest("Language", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _languageRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/Language/5
        //[HttpGet("language/{id}")]
        //public IActionResult LanguageEdit(long id)
        //{
        //    try
        //    {
        //        var output = _languageRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //[HttpPut("language/{id}")]
        //public IActionResult LanguageUpdate(long id, Language request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Language = request;
        //    object validateFields = _validationService.ValidateRequest("Language", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _languageRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //[HttpDelete("language/{id}")]
        //public IActionResult LanguageDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _languageRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Language is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Language has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion LANGUAGE


        //#region Course Level
        //// GET: api/Picklist/CourseLevel
        //[HttpGet("courselevel")]
        //public IActionResult CourseLevelList()
        //{
        //    try
        //    {
        //        var output = _levelRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Course level record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/CourseLevel
        //[HttpPost("courselevel")]
        //public IActionResult CourseLevelStore(Level request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Level = request;
        //    object validateFields = _validationService.ValidateRequest("Level", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _levelRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/CourseLevel/5
        //[HttpGet("courselevel/{id}")]
        //public IActionResult CourseLevelEdit(long id)
        //{
        //    try
        //    {
        //        var output = _levelRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// PUT: api/Picklist/CourseLevel/5
        //[HttpPut("courselevel/{id}")]
        //public IActionResult CourseLevelUpdate(long id, Level request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Level = request;
        //    object validateFields = _validationService.ValidateRequest("Level", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _levelRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //// Delete: api/Picklist/CourseLevel/5
        //[HttpDelete("courselevel/{id}")]
        //public IActionResult CourseLevelDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {

        //        var response = _levelRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course level is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Course level has been successfully deleted" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Course Level


        //#region Course Type
        //// GET: api/Picklist/CourseType
        //[HttpGet("coursetype")]
        //public IActionResult CourseTypeList()
        //{
        //    try
        //    {
        //        var output = _typesRepository.GetAll();

        //        if (output.Count() > 0)
        //            return Ok(output);
        //        else
        //            return Ok(new GenericResult { Response = false, Message = "Course Type record is empty" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// POST: api/Picklist/CourseType
        //[HttpPost("coursetype")]
        //public IActionResult CourseTypeStore(Types request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Types = request;
        //    object validateFields = _validationService.ValidateRequest("Types", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _typesRepository.Add(request);
        //        if (response == false)
        //            return BadRequest(new GenericResult { Response = response, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = response, Message = request.name + " has been successfully added" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// GET: api/Picklist/CourseType/5
        //[HttpGet("coursetype/{id}")]
        //public IActionResult CourseTypeEdit(long id)
        //{
        //    try
        //    {
        //        var output = _typesRepository.GetById(id);

        //        if (output != null)
        //            return Ok(output);
        //        else
        //            return NotFound(_NotFound);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}

        //// PUT: api/Picklist/CourseType/5
        //[HttpPut("coursetype/{id}")]
        //public IActionResult CourseTypeUpdate(long id, Types request)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    RequiredFields model = new RequiredFields();
        //    model.Types = request;
        //    object validateFields = _validationService.ValidateRequest("Types", model);
        //    if (JsonConvert.SerializeObject(validateFields).Length > 2)
        //        return BadRequest(validateFields);

        //    try
        //    {
        //        var response = _typesRepository.Update(id, request);
        //        if (response == 0)
        //            return NotFound(_NotFound);
        //        else if (response == 1)
        //            return BadRequest(new GenericResult { Response = false, Message = request.name + " already exists" });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = request.name + " has been successfully updated" });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }

        //}

        //// Delete: api/Picklist/CourseType/5
        //[HttpDelete("coursetype/{id}")]
        //public IActionResult CourseTypeDelete(long id)
        //{
        //    if (_userRepository.LogCurrentUser().role != "Administrator")
        //        return Unauthorized(_UnAuthorized);

        //    try
        //    {
        //        var response = _typesRepository.Delete(id);
        //        if (response == "not exists")
        //            return NotFound(_NotFound);
        //        else if (response == "in used")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete. Course Type is currently in used." });
        //        else if (response == "not editable")
        //            return BadRequest(new GenericResult { Response = false, Message = "Unable to delete this record." });
        //        else
        //            return Ok(new GenericResult { Response = true, Message = "Course Types has been successfully deleted" });
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        //#endregion Course Type


    }
}
