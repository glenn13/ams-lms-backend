using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        bool Add(Category request);
        Category GetById(long id);
        int Update(long id, Category request);
        string Delete(long id);
        bool Save();
    }
}
