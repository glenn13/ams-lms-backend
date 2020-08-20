using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IStatusRepository
    {
        IEnumerable<Status> GetAll();
        bool Add(Status request);
        Status GetById(long id);
        int Update(long id, Status request);
        bool Delete(long id);
        bool Save();
    }
}
