using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetAll();
        bool Add(Language request);
        Language GetById(long id);
        int Update(long id, Language request);
        string Delete(long id);
        bool Save();
    }
}
