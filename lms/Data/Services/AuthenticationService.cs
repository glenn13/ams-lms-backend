using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Schema;
using lms.Data;
using System.Collections;
using Newtonsoft.Json;

namespace lms.Data.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private lmsContext _context;
        private readonly IEncryptionService _encryptionService;

        public AuthenticationService (IOptions<AppSettings> appSettings, lmsContext context, IEncryptionService encryptionService)
        {
            _appSettings = appSettings.Value;
            _encryptionService = encryptionService;
            _context = context;
        }

        private List<Users> users = new List<Users>() { 
            new Users { id = 1, firstName = "glen", lastName = "pogi", username = "gabalos", password = "123456" }
        };
        public Users Authenticate(string username, string password)
        {
            var user = _context.Users.Where(x => x.username == username && x.isActive == 0).SingleOrDefault();
            //var user = users.SingleOrDefault(x => x.username == username && x.password == password);

            // return null if user is not found
            if (user == null || !isPasswordValid(user, password))
                return null;

            // If user is found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var accessRole = "";
            if (user.isAdministrator == 1)
                accessRole = "Admin";
            else if (user.isInstructor == 1)
                accessRole = "Instructor";
            else
                accessRole = "Learner";

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, accessRole),
                    new Claim(ClaimTypes.Version, "V3.1")
                }),
                Expires = DateTime.UtcNow.AddDays(7), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            user.password = null;

            return user;
        }

        public ClaimsDetails getUserRole (long id)
        {
            var user = _context.Users.Where(x => x.id == id).SingleOrDefault();
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
            return model;
        }

        private bool isPasswordValid(Users users, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, users.salt), users.password);
        }
    }
}
