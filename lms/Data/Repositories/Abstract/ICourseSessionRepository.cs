using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseSessionRepository
    {
        IEnumerable<CourseSession> GetAll();
        bool Add(CourseSession request);
        CourseSession GetById(long id);
        bool Update(long id, CourseSession request);
        bool Delete(long id);
        IEnumerable<CourseSession> getAllInstructorByUserId(long id);
        bool DuplicateSession(long id, CourseSession request);
        bool Save();
    }
}
