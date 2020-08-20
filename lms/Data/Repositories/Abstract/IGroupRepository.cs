using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IGroupRepository
    {
        IEnumerable<Groups> GetAll();
        bool Add(Groups request);
        Groups GetById(long id);
        int Update(long id, Groups request);
        bool Delete(long id);
        IEnumerable<Groups> SearchByUserGroup(long id);
        bool Save();
    }
}
