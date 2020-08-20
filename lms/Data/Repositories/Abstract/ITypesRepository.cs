using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ITypesRepository
    {
        IEnumerable<Types> GetAll();
        bool Add(Types request);
        Types GetById(long id);
        int Update(long id, Types request);
        string Delete(long id);
        bool Save();
    }
}
