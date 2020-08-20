using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IUserGroupRepository
    {
        IEnumerable<UserGroups> GetAll();
        bool Add(UserGroups request);
        UserGroups GetById(long id);
        int Update(long id, UserGroups request);
        bool Delete(long id);
        bool Save();
    }
}
