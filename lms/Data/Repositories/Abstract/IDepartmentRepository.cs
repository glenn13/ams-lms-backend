using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        bool Add(Department request);
        Department GetById(long id);
        int Update(long id, Department request);
        string Delete(long id);
        bool Save();
    }
}
