using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IInstructorRepository
    {
        IEnumerable<CourseInstructor> GetAll();
        IEnumerable<CourseInstructor> GetAllByCourse(long courseId);
        bool Add(CourseInstructor request);
        CourseInstructor GetById(long id);
        int Update(long id, CourseInstructor request);
        string Delete(long id);
        bool Save();
    }
}
