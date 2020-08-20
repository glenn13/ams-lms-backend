using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using lms.Data.Services;
using Microsoft.AspNetCore.Http;

namespace lms.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly lmsContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository (lmsContext context, IEncryptionService encryptionService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _encryptionService = encryptionService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Users>> GetAll()
        {
            return _context.Users.ToList();
        }

        public Users Add(Users request)
        {
            int baseId = 100000;
            var maxId = _context.Users.AsEnumerable().OrderByDescending(x => x.id).FirstOrDefault();
            int empId = baseId + Convert.ToInt16(maxId.id) + 1;

            request.empId = Convert.ToString(empId);
            request.isLearner = 1;
            request.salt = _encryptionService.CreateSalt();
            request.password = _encryptionService.EncryptPassword(request.password, request.salt);
            request.hireDate = DateTime.Now;
            request.createdAt = DateTime.Now;
            request.updatedAt = DateTime.Now;

            var users = _context.Users.Add(request);
            _context.SaveChangesAsync();
            return request;
        }

        public object UserSelection()
        {
            return _context.Users.Where(x => x.isLearner == 1 && x.isActive == 0)
                                 .Select(x => new { 
                                     userId = x.id, 
                                     empId = x.empId, 
                                     empName = String.Format("{0} {1}", x.firstName, x.lastName),
                                 })
                                 .ToList();
        }

        public Users GetById(long id)
        {
            return _context.Users.Where(x => x.id == id).FirstOrDefault();
        }

        public ClaimsDetails LogCurrentUser()
        {
            var id = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = _context.Users.Where(x => x.id == Convert.ToInt32(id)).SingleOrDefault();
            var accessRole = "";
            if (user.isAdministrator == 1)
                accessRole = "Administrator";
            else if (user.isInstructor == 1)
                accessRole = "Instructor";
            else
                accessRole = "Learner";

            bool userIsActive = false;
            if (user.isActive == 0)
                userIsActive = true;
            ClaimsDetails model = new ClaimsDetails();
            model.id = user.id;
            model.name = user.firstName + " " + user.lastName;
            model.role = accessRole;
            model.isActive = userIsActive;
            model.canCreate = (user.canCreate == 1) ? true : false;
            model.canModify = (user.canModify == 1) ? true : false;
            model.canRemove = (user.canRemove == 1) ? true : false;
            return model;
        }

        public string UpdateAsync(long id, Users request)
        {
            var users = _context.Users.Where(e => e.id == id).FirstOrDefault();
            var emailExists = _context.Users.Any(x => x.email == request.email && x.id != id);
            var usernameExists = _context.Users.Any(x => x.username == request.username && x.id != id);

            if (users == null)
                return "User not found";
            //return NotFound(new GenericResult { Response = false, Message = "User not found" });
            else if (emailExists == true)
                return "Email Address is not available";
            //return NotFound(new GenericResult { Response = false, Message = "Email Address is not available" });
            else if (usernameExists == true)
                return "Username is not available";
                //return NotFound(new GenericResult { Response = false, Message = "Username is not available" });

            users.firstName = request.firstName;
            users.middleInitial = request.middleInitial;
            users.lastName = request.lastName;
            users.username = request.username;
            users.email = request.email;
            users.projId = request.projId;
            users.deptId = request.deptId;
            users.positionId = request.positionId;
            users.internationalStatusId = request.internationalStatusId;
            users.obsId = request.obsId;
            users.branchId = request.branchId;
            users.birthday = request.birthday;
            users.gender = request.gender;
            _context.SaveChangesAsync();
            return "success";
        }
    }
}
