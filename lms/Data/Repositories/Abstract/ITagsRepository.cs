using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
 
namespace lms.Data.Repositories
{ 
    public interface ITagsRepository
    {
        IEnumerable<Tags> GetAll();
        bool Add(Tags request);
        Tags GetById(long id);
        int Update(long id, Tags request);
        string Delete(long id);
        bool Save();
    }
}
