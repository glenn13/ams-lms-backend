using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseOutlineRepository
    {
        IEnumerable<CourseOutline> GetAll();
        IEnumerable<CourseOutline> GetAllByCourseId(long courseId);
        bool Add(CourseOutline request);
        CourseOutline GetById(long id);
        int Update(long id, CourseOutline request);
        //bool AddCourseOutlineSections(long id, CourseOutline request);
        string Delete(long id);
        bool AddPrerequisite(CourseOutlinePrerequisite request);
        bool DeletePrerequisite(long id);
        bool AddMedia(CourseOutlineMedia request);
        bool DeleteMedia(long id);
        bool DeleteMilestone(long id);
        IEnumerable<CourseOutline> GetByCourseId(long id);
        bool DuplicateByCourseId(long id, long newCourseId);

        bool AddMilestone(CourseOutlineMilestone request);
        bool Save();
    }
}
