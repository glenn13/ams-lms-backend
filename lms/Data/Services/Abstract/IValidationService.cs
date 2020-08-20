using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Data.Helpers;
using lms.Models;

namespace lms.Data.Services
{
    public interface IValidationService
    {
        Users validateEmailExist(string email);
        Users validateEmpIdExist(string email);
        Users validateUsernameExist(string email);
        bool IsValidEmail(string email);
        Object registrationValidation(Users request);
        object ValidateRequest(string model, RequiredFields request);
    }
}
