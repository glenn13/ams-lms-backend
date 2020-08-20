using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAll();
        Users Add(Users request);
        object UserSelection();
        Users GetById(long id);
        ClaimsDetails LogCurrentUser();
        string UpdateAsync(long id, Users request);
    }
}
