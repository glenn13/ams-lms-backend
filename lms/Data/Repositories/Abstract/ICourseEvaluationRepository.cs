using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseEvaluationRepository
    {
        IEnumerable<CourseEvaluation> GetAll();
        IEnumerable<CourseEvaluation> GetAllByCourse(long courseId);
        bool Add(CourseEvaluation request);
        CourseEvaluation GetById(long id);
        int Update(long id, CourseEvaluation request);
        bool Delete(long id);
        bool DeleteValues(long id);
        IEnumerable<CourseEvaluation> GetByCourseId(long id);
        bool DuplicateByCourseId(long id, long newCourseId);
        bool Save();
    }
}
