using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetAll();
        bool Add(Location request);
        Location GetById(long id);
        int Update(long id, Location request);
        string Delete(long id);
        bool Save();
    }
}
