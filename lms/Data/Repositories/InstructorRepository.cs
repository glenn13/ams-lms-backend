using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly lmsContext _context;
        public InstructorRepository (lmsContext context)
        {
            _context = context;
        }
        public IEnumerable<CourseInstructor> GetAll()
        {
            return _context.CourseInstructor.Include(x => x.Usergroup).ToList();
        }
        public IEnumerable<CourseInstructor> GetAllByCourse(long courseId)
        {
            return _context.CourseInstructor.Include(x => x.Usergroup).Where(x => x.courseId == courseId).ToList();
        }

        public bool Add (CourseInstructor request)
        {
            var output = false;
            var validate = _context.CourseInstructor.Where(x => x.userId == request.userId && x.courseId == request.courseId).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                var model = _context.CourseInstructor.Add(request);
                Save();
                output = true;
            }
            return output;

        }

        public CourseInstructor GetById(long id)
        {
            return _context.CourseInstructor.Where(x => x.id == id).Include(x => x.Course).Include(x => x.Usergroup).FirstOrDefault();
        }

        public int Update(long id, CourseInstructor request)
        {

            CourseInstructor model = GetById(id);
            var existingInsturctorInUsergroup = _context.CourseInstructor.Where(x => x.userId == request.userId && x.courseId == request.courseId && x.id != id).FirstOrDefault();
            if (model == null)
                return 0;
            else if (existingInsturctorInUsergroup != null)
                return 1;

            //var model = _context.Instructor.Where(x => x.id == id).FirstOrDefault();
            model.userId = request.userId;
            model.userGroupId = request.userGroupId;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }


        public string Delete(long id)
        {
            var model = _context.CourseInstructor.Where(x => x.id == id).FirstOrDefault();
            var counterSession = _context.CourseSession.Where(x => x.courseInstructorId == id).Count();
            if (model == null)
                return "not exists";

            if (counterSession > 0)
                return "in used";

            _context.CourseInstructor.Remove(model);
            Save();
            return "deleted";
        }

        public bool Save ()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
