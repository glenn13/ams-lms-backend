using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseAssessmentRepository
    {
        IEnumerable<CourseAssessment> GetAll();
        IEnumerable<CourseAssessment> GetAllByCourse(long courseId);
        bool Add(CourseAssessment request);
        CourseAssessment GetById(long id);
        int Update(long id, CourseAssessment request);
        string Delete(long id);
        bool DeleteItem(long id);
        bool DeleteItemChoices(long id);
        IEnumerable<CourseAssessment> GetByCourseId(long id);
        bool DuplicateByCourseId(long id, long newCourseId);
        bool Save();
    }
}
