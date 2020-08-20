using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseOutcomeRepository
    {
        IEnumerable<CourseOutcome> GetAll();
        IEnumerable<CourseOutcome> GetAllByCourseId(long courseId);
        bool Add(CourseOutcome request);
        CourseOutcome GetById(long id);
        int Update(long id, CourseOutcome request);
        bool Delete(long id);
        bool DuplicateByCourseId(long id, long newCourseId);
        bool Save();
    }
}
