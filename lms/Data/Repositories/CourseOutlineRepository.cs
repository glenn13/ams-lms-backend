using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using lms.Models;
using lms.Data.Core;
using lms.Data;
using System.IO;

namespace lms.Data.Repositories
{
    public class CourseOutlineRepository : ICourseOutlineRepository
    {
        private readonly lmsContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileDirectory _fileDirectory;

        public CourseOutlineRepository(lmsContext context,
                                       IFileDirectory fileDirectory,
                                       IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _fileDirectory = fileDirectory;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<CourseOutline> GetAll()
        {
            return _context.CourseOutline.Include(a => a.Course)
                                         .Include(b => b.UserGroup)
                                         .ToList();
        }

        public IEnumerable<CourseOutline> GetAllByCourseId(long courseId)
        {
            return _context.CourseOutline.Include(x => x.Course)
                                         .Include(x => x.UserGroup)
                                         .Where(x => x.courseId == courseId)
                                         .ToList();
        }

        public bool Add(CourseOutline request)
        {
            var output = false;
            var validate = isExists(request);
            if (validate == false)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                _context.CourseOutline.Add(request);
                Save();
                output = true;
            }
            return output;
        }


        public CourseOutline GetById(long id)
        {
            return _context.CourseOutline.Where(x => x.id == id)
                                         .Include(a => a.Course)
                                         .Include(b => b.UserGroup)
                                         //.Include(c => c.CourseOutlinePrerequisite)
                                         //.Include(d => d.CourseOutlineMedia)
                                         //.Include(e => e.CourseOutlineMilestone)
                                         .FirstOrDefault();
        }

        public int Update(long id, CourseOutline request)
        {
            CourseOutline model = GetById(id);
            var validate = isExistsById(id, request);

            if (model == null)
                return 0;
            else if (validate == true)
                return 1;

            model.title = request.title;
            model.courseId = request.courseId;
            model.userGroupId = request.userGroupId;
            model.visibility = request.visibility;
            model.featureImage = request.featureImage;
            model.interactiveVideo = request.interactiveVideo;
            model.duration = request.duration;
            model.description = request.description;
            model.updatedAt = DateTime.Now;
            Save();

            return 2;
        }


        public string Delete(long id)
        {
            var model = _context.CourseOutline.Where(x => x.id == id).Include(x => x.LearnerCourseOutline).FirstOrDefault();
            if (model == null)
                return "not exists";

            if (model.LearnerCourseOutline.Count() > 0)
                return "in used";


            var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
            string courseOutlineImagesFolder = String.Format("{0}\\Content\\Images\\CourseOutline", path);
            string courseOutlineVideoFolder = String.Format("{0}\\Content\\Video\\CourseOutline", path);

            if (System.IO.File.Exists(Path.Combine(courseOutlineImagesFolder, model.featureImage)))
            {
                System.IO.File.Delete(Path.Combine(courseOutlineImagesFolder, model.featureImage));
            }

            if (System.IO.File.Exists(Path.Combine(courseOutlineVideoFolder, model.interactiveVideo)))
            {
                System.IO.File.Delete(Path.Combine(courseOutlineVideoFolder, model.interactiveVideo));
                System.IO.File.Delete(Path.Combine(courseOutlineVideoFolder, model.interactiveVideo+".zip"));
            }


            DeletePrerequisiteByCourseOutline(id);

            DeleteMilestoneByCourseOutline(id);

            DeleteMediaByCourseOutline(id);

            _context.CourseOutline.Remove(model);
            Save();
            return "deleted";
        }

        public bool AddPrerequisite(CourseOutlinePrerequisite request)
        {
            request.createdAt = DateTime.Now;
            request.updatedAt = DateTime.Now;
            _context.CourseOutlinePrerequisite.Add(request);
            Save();

            return true;
        }

        public bool DeletePrerequisite(long id)
        {
            var output = false;
            var model = _context.CourseOutlinePrerequisite.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseOutlinePrerequisite.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeletePrerequisiteByCourseOutline(long id)
        {
            var output = false;
            var model = _context.CourseOutlinePrerequisite.Where(x => x.courseOutlineId == id).ToList();
            if (model != null)
            {
                _context.CourseOutlinePrerequisite.RemoveRange(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool AddMedia(CourseOutlineMedia request)
        {

            request.createdAt = DateTime.Now;
            request.updatedAt = DateTime.Now;
            _context.CourseOutlineMedia.Add(request);
            Save();

            return true;
        }

        public bool DeleteMedia(long id)
        {
            var output = false;
            var model = _context.CourseOutlineMedia.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                string courseOutlineMediaFolder = String.Format("{0}\\Content\\Images\\CourseOutlineMedia", path);

                if (!Directory.Exists(courseOutlineMediaFolder))
                    Directory.CreateDirectory(courseOutlineMediaFolder);

                if (System.IO.File.Exists(Path.Combine(courseOutlineMediaFolder, model.resourceFile)))
                {
                    System.IO.File.Delete(Path.Combine(courseOutlineMediaFolder, model.resourceFile));
                }

                _context.CourseOutlineMedia.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }
        private bool DeleteMediaByCourseOutline(long id)
        {
            var output = false;
            var model = _context.CourseOutlineMedia.Where(x => x.courseOutlineId == id).ToList();
            if (model != null)
            {
                foreach (CourseOutlineMedia i in model)
                {
                    var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                    string courseOutlineMediaFolder = String.Format("{0}\\Content\\Images\\CourseOutlineMedia", path);

                    if (System.IO.File.Exists(Path.Combine(courseOutlineMediaFolder, i.resourceFile)))
                    {
                        System.IO.File.Delete(Path.Combine(courseOutlineMediaFolder, i.resourceFile));
                    }

                    _context.CourseOutlineMedia.Remove(i);
                    _context.SaveChanges();
                }
                output = true;
            }
            return output;
        }

        public bool AddMilestone(CourseOutlineMilestone request)
        {

            var output = false;
            var validate = isExistsMilestone(request);
            if (validate == false)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                _context.CourseOutlineMilestone.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public bool DeleteMilestone(long id)
        {
            var output = false;
            var model = _context.CourseOutlineMilestone.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseOutlineMilestone.Remove(model);
                _context.SaveChanges();

                output = true;

            }
            return output;
        }
        private bool DeleteMilestoneByCourseOutline(long id)
        {
            var output = false;
            var model = _context.CourseOutlineMilestone.Where(x => x.courseOutlineId == id).ToList();
            if (model != null)
            {
                foreach (CourseOutlineMilestone i in model)
                {
                    var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                    string courseOutlineMilestoneFolder = String.Format("{0}\\Content\\Images\\CourseOutlineMilestone", path);

                    if (System.IO.File.Exists(Path.Combine(courseOutlineMilestoneFolder, i.resourceFile)))
                    {
                        System.IO.File.Delete(Path.Combine(courseOutlineMilestoneFolder, i.resourceFile));
                    }

                    _context.CourseOutlineMilestone.Remove(i);
                    _context.SaveChanges();
                }
                output = true;
            }
            return output;
        }



        public IEnumerable<CourseOutline> GetByCourseId(long id)
        {
            return _context.CourseOutline.Where(x => x.courseId == id)
                                         .ToList();
        }

        public bool DuplicateByCourseId(long id, long newCourseId)
        {

            string sqlCourseOutline = String.Format("INSERT INTO [course].[course_outline] (title, courseId, userGroupId, visibility, featureImage, interactiveVideo, duration, description, createdAt, updatedAt) " +
                                                  "SELECT title, CONCAT('','','{0}') AS courseId, userGroupId, visibility, featureImage, interactiveVideo, duration, description, createdAt, updatedAt FROM [course].[course_outline] WHERE courseId = {1}", newCourseId, id);
            _context.Database.ExecuteSqlCommand(sqlCourseOutline);

            string sqlCourseOutlinemedia = String.Format("INSERT INTO [course].[course_outline_media] (courseId, resourceFile, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, resourceFile, createdAt, updatedAt FROM [course].[course_outline_media] WHERE courseId = {1}", newCourseId, id);
            _context.Database.ExecuteSqlCommand(sqlCourseOutlinemedia);

            string sqlCourseOutlineMilestone = String.Format("INSERT INTO [course].[course_outline_milestone] (courseId, name, lessonCompleted, resourceFile, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, name, lessonCompleted, resourceFile, createdAt, updatedAt FROM [course].[course_outline_milestone] WHERE courseId = {1}", newCourseId, id);
            _context.Database.ExecuteSqlCommand(sqlCourseOutlineMilestone);

            string sqlCourseOutlinePrerequisite = String.Format("INSERT INTO [course].[course_outline_prerequisite] (courseId, preRequisiteId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, preRequisiteId, createdAt, updatedAt FROM [course].[course_outline_prerequisite] WHERE courseId = {1}", newCourseId, id);
            _context.Database.ExecuteSqlCommand(sqlCourseOutlinePrerequisite);



            //var courseOutline = GetByCourseId(id);

            //foreach (CourseOutline co in courseOutline)
            //{
            //    CourseOutline coModel = new CourseOutline();
            //    coModel.title = co.title;
            //    coModel.courseId = newCourseId;
            //    coModel.userGroupId = co.userGroupId;
            //    coModel.visibility = co.visibility;
            //    coModel.featureImage = co.featureImage;
            //    coModel.interactiveVideo = co.interactiveVideo;
            //    coModel.duration = co.duration;
            //    coModel.description = co.description;
            //    coModel.createdAt = co.createdAt;
            //    coModel.updatedAt = co.updatedAt;

            //    _context.CourseOutline.Add(coModel);
            //    _context.SaveChanges();

            //}

            return true;
        }






        private bool isExistsMilestone(CourseOutlineMilestone request)
        {
            return _context.CourseOutlineMilestone.Where(x => x.name == request.name && x.courseId == request.courseId).Any();
        }
        private bool isExists(CourseOutline request)
        {
            return _context.CourseOutline.Where(x => x.title == request.title && x.courseId == request.courseId).Any();
        }

        private bool isExistsById(long id, CourseOutline request)
        {
            return _context.CourseOutline.Where(x => x.title == request.title && x.userGroupId == request.userGroupId && x.id != id).Any();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
