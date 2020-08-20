using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ISessionTypeRepository
    {
        IEnumerable<SessionType> GetAll();
        bool Add(SessionType request);
        SessionType GetById(long id);
        int Update(long id, SessionType request);
        string Delete(long id);
        bool Save();
    }
}
