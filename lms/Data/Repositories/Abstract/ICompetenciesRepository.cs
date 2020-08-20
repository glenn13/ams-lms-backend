using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICompetenciesRepository
    {
        IEnumerable<CourseCompetencies> GetAll();
        IEnumerable<CourseCompetencies> GetAllByCourse(long courseId);
        bool Add(CourseCompetencies request);
        CourseCompetencies GetById (long id);
        int Update(long id, CourseCompetencies request);
        bool Delete(long id);
        IEnumerable<CourseCompetencies> GetByCourseId(long id);
        bool DuplicateByCourseId(long id, long newCourseId);
        bool Save();
    }
}
