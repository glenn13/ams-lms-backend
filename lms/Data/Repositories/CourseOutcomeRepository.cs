using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;
using lms.Data;

namespace lms.Data.Repositories
{
    public class CourseOutcomeRepository : ICourseOutcomeRepository
    {
        private readonly lmsContext _context;
        public CourseOutcomeRepository (lmsContext context)
        {
            _context = context;
        }
        public IEnumerable<CourseOutcome> GetAll()
        {
            return _context.CourseOutcome.Include(x => x.UserGroup)
                                         .ToList();
        }
        public IEnumerable<CourseOutcome> GetAllByCourseId(long courseId)
        {
            return _context.CourseOutcome.Include(x => x.UserGroup)
                                         .Where(x => x.courseId == courseId)
                                         .ToList();
        }

        public bool Add (CourseOutcome request)
        {
            var output = false;
            var validate = isExists(request);
            if (validate == false)
            {
                //var output = false;
                _context.CourseOutcome.Add(request);
                _context.SaveChanges();
                output = true;
            }
            return output;
        }

        public CourseOutcome GetById (long id)
        {
            return _context.CourseOutcome.Where(x => x.id == id)
                                         .Include(x => x.UserGroup)
                                         .FirstOrDefault();
        }

        public int Update(long id, CourseOutcome request)
        {
            CourseOutcome model = GetById(id);
            var validate = isExistsById(id, request);

            if (model == null)
                return 0;
            else if (validate == true)
                return 1;


            model.title = request.title;
            model.courseId = request.courseId;
            model.userGroupId = request.userGroupId;
            model.visibility = request.visibility;
            model.description = request.description;
            model.updatedAt = DateTime.Now;
            Save();

            return 2;

        }

        public bool Delete(long id)
        {
            var output = false;
            var model = _context.CourseOutcome.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseOutcome.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;

        }

        public bool DuplicateByCourseId(long id, long newCourseId)
        {

            string sqlCourseOutcome = String.Format("INSERT INTO [course].[course_outcome] (title, courseId, userGroupId, visibility, description, createdAt, updatedAt) " +
                                                  "SELECT title, CONCAT('','','{0}') AS courseId, userGroupId, visibility, description, createdAt, updatedAt FROM [course].[course_outcome] WHERE courseId = {1}", newCourseId, id);
            _context.Database.ExecuteSqlCommand(sqlCourseOutcome);

            return true;
        }



        private bool isExists(CourseOutcome request)
        {
            return _context.CourseOutcome.Where(x => x.title == request.title && x.userGroupId == request.userGroupId).Any();
        }
        private bool isExistsById(long id, CourseOutcome request)
        {
            return _context.CourseOutcome.Where(x => x.title == request.title && x.userGroupId == request.userGroupId && x.id != id).Any();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
