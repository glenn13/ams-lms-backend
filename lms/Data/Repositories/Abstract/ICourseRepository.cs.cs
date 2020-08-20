using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using lms.Data.Helpers;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ICourseRepository
    {

        //IList<Courses> GetAll();
        IEnumerable<Courses> GetAll();
        bool Add(Courses request);
        Courses GetById(long id);
        bool Update(long id, Courses request);
        string Delete(long id);
        bool DeleteCourseLevel(long id);
        bool DeleteCourseCategory(long id);
        bool DeleteCourseType(long id);
        bool DeleteCourseRelated(long id);
        bool DeleteCourseLanguage(long id);
        bool DeleteCouseTag(long id);
        bool RequestPublishCourse(PublishCourse request);
        bool PublishCourse(PublishCourse request);
        bool UnpublishCourse(PublishCourse request);
        bool AddCourseSections(long id, Courses request);
        Courses GetByIdCourseSimple(long id);
        Courses GetByName(string title);
        bool DuplicateCourse(long id, string refCode, string title);
        Courses GetByCode(string code);
        bool Save();
    }
}
