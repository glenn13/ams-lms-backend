using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;
using System.Collections;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace lms.Data.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly lmsContext _context;
        public EnrollmentRepository(lmsContext context)
        {
            _context = context;
        }
        public object GetAll(long userId = 0)
        {

            var myClass = _context.Users.Where(x => x.isLearner == 1)
                                        .Include(x => x.Learner)
                                        .Select(i => new {
                                            empId = i.empId,
                                            firstName = i.firstName,
                                            middleInitial = i.middleInitial,
                                            lastName = i.lastName,
                                            username = i.username, 
                                            email = i.email, 
                                            Learner = i.Learner.Select(l => new { 
                                                learnerId = l.id, 
                                                courseId = l.courseId,
                                                enrollmentType = l.enrollmentType,
                                                statusId = l.statusId, 
                                                statusName = l.Status.name, 
                                                statusColor = l.Status.color,
                                                assessmentStatusId = l.assessmentStatusId,
                                                isRecommendCourse = l.isRecommendCourse,
                                                courseRating = l.courseRating,
                                                courseReview = l.courseReview,
                                                finalScore = l.finalScore,
                                                totalHoursTaken = l.totalHoursTaken,
                                                isApproved = l.isApproved,
                                                startDate = l.startDate,
                                                endDate = l.endDate,
                                                overallRating = l.overallRating
                                            })
                                        })
                                        .ToList();
            return myClass;
        }

        public Learner GetById(long id)
        {
            return _context.Learner.Where(x => x.id == id)
                                   .Include(x => x.LearnerSession)
                                   .Include(x => x.Course)
                                        .ThenInclude(x => x.CourseInstructor)
                                   .FirstOrDefault();
        }


        public bool DeleteEnrollee(long id)
        {
            var output = false;
            var model = _context.Learner.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.Learner.Remove(model);
                Save();

                output = true;
            }
            return output;
        }



        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }



    }
}
