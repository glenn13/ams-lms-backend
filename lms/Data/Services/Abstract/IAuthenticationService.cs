using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Services
{
    public interface IAuthenticationService
    {
        Users Authenticate(string username, string password);
    }
}
