using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ILevelRepository
    {
        IEnumerable<Level> GetAll();
        bool Add(Level request);
        Level GetById(long id);
        int Update(long id, Level request);
        string Delete(long id);
        bool Save();
    }
}
