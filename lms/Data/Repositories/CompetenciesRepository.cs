using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class CompetenciesRepository : ICompetenciesRepository
    {
        private readonly lmsContext _context;
        public CompetenciesRepository (lmsContext context)
        {
            _context = context;
        }
        public IEnumerable<CourseCompetencies> GetAll() 
        {
            return _context.CourseCompetencies.Include(x => x.UserGroup)
                                        .Include(x => x.CourseCompetenciesCertificate)
                                        .ToList();
        }

        public IEnumerable<CourseCompetencies> GetAllByCourse(long courseId)
        {
            return _context.CourseCompetencies.Include(x => x.UserGroup)
                                        .Include(x => x.CourseCompetenciesCertificate)
                                        .Where(x => x.courseId == courseId)
                                        .ToList();
        }

        public bool Add (CourseCompetencies request)
        {
            var output = false;
            var validate = _context.CourseCompetencies.Where(x => x.title == request.title && x.courseId == request.courseId).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                var model = _context.CourseCompetencies.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public CourseCompetencies GetById(long id)
        {
            return _context.CourseCompetencies.Where(x => x.id == id)
                                        .Include(x => x.UserGroup)
                                        .Include(x => x.CourseCompetenciesCertificate)
                                        .FirstOrDefault();
        }

        public int Update(long id, CourseCompetencies request)
        {
            var model = GetById(id);
            var exists = isExists(id, request);

            if (model == null)
                return 0;
            else if (exists == true)
                return 1;


            model.title = request.title;
            model.description = request.description;
            model.userGroupId = request.userGroupId;
            model.lessonCompleted = request.lessonCompleted;
            model.milestonesReached = request.milestonesReached;
            model.assessmentsSubmitted = request.assessmentsSubmitted;
            model.updatedAt = DateTime.Now;
            Save();

            return 2;
        }

        public bool Delete(long id)
        {
            var output = false;

            var model = _context.CourseCompetencies.Where(x => x.id == id).Include(a => a.CourseCompetenciesCertificate).FirstOrDefault();

            if (model != null)
            {
                _context.CourseCompetencies.Remove(model);
                Save();
                output = true;
            }

            return output;
        }
        public IEnumerable<CourseCompetencies> GetByCourseId(long id)
        {
            return _context.CourseCompetencies.Where(x => x.courseId == id)
                                        .Include(x => x.UserGroup)
                                        .Include(x => x.CourseCompetenciesCertificate)
                                         .ToList();
        }




        public bool DuplicateByCourseId(long id, long newCourseId)
        {

            var courseCompetencies = GetByCourseId(id);

            foreach (CourseCompetencies cc in courseCompetencies)
            {
                CourseCompetencies ccModel = new CourseCompetencies();
                ccModel.courseId = newCourseId;
                ccModel.title = cc.title;
                ccModel.description = cc.description;
                ccModel.userGroupId = cc.userGroupId;
                ccModel.lessonCompleted = cc.lessonCompleted;
                ccModel.milestonesReached = cc.milestonesReached;
                ccModel.assessmentsSubmitted = cc.assessmentsSubmitted;
                ccModel.final = cc.final;
                ccModel.createdAt = DateTime.Now;
                ccModel.updatedAt = DateTime.Now;
                _context.CourseCompetencies.Add(ccModel);
                _context.SaveChanges();

                foreach (CourseCompetenciesCertificate ccc in cc.CourseCompetenciesCertificate)
                {
                    CourseCompetenciesCertificate cccModel = new CourseCompetenciesCertificate();
                    cccModel.courseCompetenciesId = ccModel.id;
                    cccModel.attachment = ccc.attachment;
                    cccModel.createdAt = DateTime.Now;
                    cccModel.updatedAt = DateTime.Now;
                    _context.CourseCompetenciesCertificate.Add(cccModel);
                    _context.SaveChanges();
                }
            }
            return true;
        }


        private bool isExists(long id, CourseCompetencies request)
        {
            return _context.CourseCompetencies.Any(e => e.id != id && e.title == request.title && e.courseId == request.courseId);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
