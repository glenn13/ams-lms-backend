using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;
using System.Collections;
using lms.Data.Helpers;

namespace lms.Data.Repositories
{
    public class CourseSessionRepository : ICourseSessionRepository
    {
        private readonly lmsContext _context;
        public CourseSessionRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<CourseSession> GetAll()
        {
            return _context.CourseSession.Include(x => x.Course)
                                         .Include(x => x.SessionType)
                                         .Include(x => x.UserGroup)
                                         .Include(x => x.CourseInstructor)
                                         .ToList();
        }

        public bool Add(CourseSession request)
        {
            //var output = false;
            //return output;
            request.createdAt = DateTime.Now;
            _context.CourseSession.Add(request);
            Save();
            return true;
            //await _context.SaveChangesAsync();
        }
         
        public CourseSession GetById(long id)
        {
            return _context.CourseSession.Where(x => x.id == id)
                                         .Include(x => x.Course)
                                         .Include(x => x.SessionType)
                                         .Include(x => x.UserGroup)
                                         .Include(x => x.CourseInstructor)
                                         .FirstOrDefault();
        }


        public bool Update(long id, CourseSession request)
        {
            var output = false;
            CourseSession model = GetById(id);
            if (model != null)
            {
                model.courseId = request.courseId;
                model.sessionTypeId = request.sessionTypeId;
                model.sessionLocation = request.sessionLocation;
                model.userGroupId = request.userGroupId;
                model.capacity = request.capacity;
                model.startDate = request.startDate;
                model.endDate = request.endDate;
                model.duration = request.duration;
                model.courseInstructorId = request.courseInstructorId;
                model.title = request.title;
                model.description = request.description;
                model.updatedAt = DateTime.Now;
                Save();
                output = true;
            }
            return output;
        }

        public IEnumerable<CourseSession> getAllInstructorByUserId(long id)
        {
            return _context.CourseSession.Where(x => x.CourseInstructor.userId == id)
                                         .Include(x => x.Course)
                                         .Include(x => x.SessionType)
                                         .Include(x => x.UserGroup)
                                         .Include(x => x.CourseInstructor)
                                         .ToList();
        }

        public bool DuplicateSession(long id, CourseSession request)
        {
            var output = false;
            var currentRecord = _context.CourseSession.Where(x => x.id == id)
                                         .FirstOrDefault();
            if (currentRecord != null)
            {
                CourseSession newRequest = new CourseSession();
                newRequest = request;
                newRequest.id = 0;
                newRequest.createdAt = DateTime.Now;
                newRequest.updatedAt = DateTime.Now;


                _context.CourseSession.Add(newRequest);
                _context.SaveChanges();
                output = true;
            }
            return output;
        }

        public bool Delete(long id)
        {
            var output = false;
            var model = _context.CourseSession.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseSession.Remove(model);
                Save();

                output = true;
            }
            return output;
        }
        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        private bool isExists(CourseSession request)
        {
            return _context.CourseSession.Where(x => x.id == request.id).Any();
        }

    }
}
