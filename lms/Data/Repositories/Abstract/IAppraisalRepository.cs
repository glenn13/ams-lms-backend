using lms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.Data.Repositories
{
    public interface IAppraisalRepository
    {
        IEnumerable<Appraisal> GetAll();
        bool Add(Appraisal request);
        Appraisal GetById(long id);
        int Update(long id, Appraisal request);
        string Delete(long id);
        bool Save();
    }
}
