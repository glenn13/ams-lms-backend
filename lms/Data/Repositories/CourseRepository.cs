using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using lms.Models;
using lms.Data;
using lms.Data.Helpers;
using lms.Data.Services;
using lms.Data.Core;
using System.IO;

namespace lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {

        private readonly lmsContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IFileDirectory _fileDirectory;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CourseRepository (lmsContext context, 
                                 IEncryptionService encryptionService,
                                 IFileDirectory fileDirectory, 
                                 IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _encryptionService = encryptionService;
            _hostingEnvironment = hostingEnvironment;
            _fileDirectory = fileDirectory;
        }


        public IEnumerable<Courses> GetAll()
        {
            return _context.Courses
                    .Include(i => i.CourseLevel)
                        .ThenInclude(i => i.level)
                    .Include(b => b.CourseType)
                        .ThenInclude(b => b.courseType)
                    .Include(c => c.RelatedCourse)
                        .ThenInclude(c => c.courseRelated)
                    .Include(d => d.CourseLanguage)
                        .ThenInclude(d => d.language)
                    .Include(e => e.CourseTag)
                        .ThenInclude(e => e.Tag)
                    .ToList();
        }

        public Courses GetById(long id)
        {
            return _context.Courses.Where(x => x.id == id)
                                    .Include(a => a.CourseLevel)
                                        .ThenInclude(a => a.level)
                                    .Include(b => b.CourseCategory)
                                        .ThenInclude(b => b.category)
                                    .Include(c => c.CourseType)
                                        .ThenInclude(c => c.courseType)
                                    .Include(d => d.CourseOutline)
                                    .Include(e => e.CourseOutcome)
                                        .ThenInclude(e => e.UserGroup)
                                    .Include(f => f.CourseCompetencies)
                                        .ThenInclude(f => f.CourseCompetenciesCertificate)
                                    .Include(g => g.RelatedCourse)
                                        .ThenInclude(g => g.courseRelated)
                                            .ThenInclude(g => g.Course)
                                    .Include(h => h.CourseLanguage)
                                        .ThenInclude(h => h.language)
                                    .Include(i => i.CourseTag)
                                        .ThenInclude(i => i.Tag)
                                    .Include(j => j.CourseInstructor)
                                        .ThenInclude(j => j.User)
                                    .Include(k => k.Learner)
                                        .ThenInclude(k => k.User)
                                    .FirstOrDefault();
        }

        public bool Add (Courses request)
        {
            var exists = isExists(request);
            if (exists == true)
                return false;

            var refCode = _encryptionService.GenerateRefCode();

            Courses model = new Courses();
            model.title = request.title;
            model.code = refCode;
            model.description = request.description;
            model.status = request.status;
            model.durationTime = request.durationTime;
            model.durationType = request.durationType;
            model.passingGrade = request.passingGrade;
            model.capacity = request.capacity;
            model.CourseLevel = request.CourseLevel;
            model.CourseCategory = request.CourseCategory;
            model.CourseType = request.CourseType;
            model.CourseLanguage = request.CourseLanguage;
            model.CourseTag = request.CourseTag;
            model.CourseInstructor = request.CourseInstructor;
            model.createdAt = DateTime.Now;

            _context.Courses.Add(model);
            Save();

            if (request.RelatedCourse != null && request.RelatedCourse.Count() > 0)
            {
                foreach (CourseRelatedDetails x in request.RelatedCourse)
                {
                    CourseRelatedDetails cr = new CourseRelatedDetails();
                    cr.courseId = model.id;
                    cr.isPrerequisite = x.isPrerequisite;
                    _context.CourseRelated.Add(cr);
                    _context.SaveChanges();

                    CourseRelatedList rcl = new CourseRelatedList();
                    rcl.courseRelatedId = cr.id;
                    rcl.courseId = x.relatedCourseId;
                    _context.CourseRelatedList.Add(rcl);
                    _context.SaveChanges();
                }
            }


            return true;
        }

        public bool Update (long id, Courses request)
        {
            var exists = isExistsById(id, request);
            if (exists == true)
                return false;

            Courses model = GetById(id);
            model.title = request.title;
            model.description = request.description;
            model.durationTime = request.durationTime;
            model.durationType = request.durationType;
            model.passingGrade = request.passingGrade;
            model.capacity = request.capacity;
            model.updatedAt = DateTime.Now;
            Save();

            AddCourseSections(id, request);

            return true;
        }


        public string Delete (long id)
        {
            var counterLearner = _context.Learner.Where(x => x.courseId == id).Count();
            var counterRelatedCourseList = _context.CourseRelatedList.Where(x => x.courseId == id).Count();
            var counterSession = _context.CourseSession.Where(x => x.courseId == id).Count();


            // COURSES
            var courses = _context.Courses.Where(x => x.id == id)
                                          .Include(x => x.CourseLevel)
                                          .Include(x => x.CourseCategory)
                                          .Include(x => x.CourseType)
                                          .Include(x => x.CourseLanguage)
                                          .Include(x => x.CourseTag)
                                          .Include(x => x.RelatedCourse)
                                            .ThenInclude(x => x.courseRelated)
                                          .FirstOrDefault();

            if (courses == null)
                return "not exists";


            if (counterLearner > 0 || counterRelatedCourseList > 0 || counterSession > 0)
                return "in used";

            var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
            string courseImagesFolder = String.Format("{0}\\Content\\Images\\Course", path);
            string courseVideoFolder = String.Format("{0}\\Content\\Video\\Course", path);

            if (System.IO.File.Exists(Path.Combine(courseImagesFolder, courses.featureImage)))
            {
                System.IO.File.Delete(Path.Combine(courseImagesFolder, courses.featureImage));
            }

            if (System.IO.File.Exists(Path.Combine(courseVideoFolder, courses.featureVideo)))
            {
                System.IO.File.Delete(Path.Combine(courseVideoFolder, courses.featureVideo));
                System.IO.File.Delete(Path.Combine(courseVideoFolder, courses.featureVideo + ".zip"));
            }

            _context.Courses.RemoveRange(courses);
            _context.SaveChanges();

            var courseRelatedList = _context.CourseRelatedList.Where(x => x.courseId == id).ToList();
            foreach (CourseRelatedList x in courseRelatedList)
            {
                var courseRelatedDetails = _context.CourseRelated.Where(i => i.id == x.courseRelatedId).Include(i => i.courseRelated).FirstOrDefault();
                _context.CourseRelated.Remove(courseRelatedDetails);
                _context.SaveChanges();
            }
            return "deleted";
        }



        public bool DeleteCourseLevel(long id)
        {
            var output = false;
            var model = _context.CourseLevel.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseLevel.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeleteCourseCategory(long id)
        {
            var output = false;
            var model = _context.CourseCategory.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseCategory.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeleteCourseType(long id)
        {
            var output = false;
            var model = _context.CourseType.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseType.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeleteCourseRelated(long id)
        {
            var output = false;
            var model = _context.CourseRelated.Where(x => x.id == id).Include(x => x.courseRelated).FirstOrDefault();
            if (model != null)
            {
                _context.CourseRelated.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeleteCourseLanguage(long id)
        {
            var output = false;
            var model = _context.CourseLanguage.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseLanguage.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool DeleteCouseTag(long id)
        {
            var output = false;
            var model = _context.CourseTag.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseTag.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }

        public bool RequestPublishCourse(PublishCourse request)
        {
            var course = GetById(request.courseId);
            if (request.courseId == 0 || course == null)
                return false;

            var model = _context.Courses.Where(x => x.id == request.courseId).FirstOrDefault();
            model.requestForPublish = 1;
            _context.SaveChanges();
            return true;
        }

        public bool PublishCourse(PublishCourse request)
        {
            var output = false;
            if (request.courseId != 0)
            {
                var model = _context.Courses.Where(x => x.id == request.courseId).FirstOrDefault();
                model.isPublished = 1;
                model.publishDescription = request.publishDescription;
                model.lmsProfile = request.lmsProfile;
                model.isVisible = request.isVisible;
                model.notifyInstructor = request.notifyInstructor;
                _context.SaveChanges();
                output = true;
            }
            return output;
        }

        public bool UnpublishCourse(PublishCourse request)
        {
            var output = false;
            if (request.courseId != 0)
            {
                var model = _context.Courses.Where(x => x.id == request.courseId).FirstOrDefault();
                model.isPublished = 0;
                model.publishDescription = "";
                model.lmsProfile = 0;
                model.notifyInstructor = 0;
                _context.SaveChanges();
                output = true;
            }
            return output;
        }
        

        public bool AddCourseSections (long id, Courses request)
        {
            var output = false;
            if (request.CourseLevel != null)
            {
                foreach (CourseLevel x in request.CourseLevel)
                {
                    x.courseId = id;
                    x.createdAt = DateTime.Now;
                    x.updatedAt = DateTime.Now;
                    _context.CourseLevel.Add(x);
                }
                _context.SaveChanges();
            }

            if (request.CourseCategory != null)
            {
                foreach (CourseCategory cc in request.CourseCategory)
                {
                    cc.courseId = id;
                    cc.createdAt = DateTime.Now;
                    cc.updatedAt = DateTime.Now;
                    _context.CourseCategory.Add(cc);
                }
                _context.SaveChanges();
            }

            if (request.CourseType != null)
            {
                foreach (CourseType i in request.CourseType)
                {
                    i.courseId = id;
                    i.createdAt = DateTime.Now;
                    i.updatedAt = DateTime.Now;
                    _context.CourseType.Add(i);
                }
                _context.SaveChanges();
            }

            if (request.RelatedCourse != null)
            {
                foreach (CourseRelatedDetails cr in request.RelatedCourse)
                {
                    cr.courseId = id;
                    cr.createdAt = DateTime.Now;
                    cr.updatedAt = DateTime.Now;
                    _context.CourseRelated.Add(cr);
                    _context.SaveChanges();

                    CourseRelatedList rcl = new CourseRelatedList();
                    rcl.courseRelatedId = cr.id;
                    rcl.courseId = cr.relatedCourseId;
                    _context.CourseRelatedList.Add(rcl);
                    _context.SaveChanges();
                }
                _context.SaveChanges();
            }

            if (request.CourseLanguage != null)
            {
                foreach (CourseLanguage cl in request.CourseLanguage)
                {
                    cl.courseId = id;
                    cl.createdAt = DateTime.Now;
                    cl.updatedAt = DateTime.Now;
                    _context.CourseLanguage.Add(cl);
                }
                _context.SaveChanges();
            }

            if (request.CourseTag != null)
            {
                foreach (CourseTag ct in request.CourseTag)
                {
                    ct.courseId = id;
                    ct.createdAt = DateTime.Now;
                    ct.updatedAt = DateTime.Now;
                    _context.CourseTag.Add(ct);
                }
                _context.SaveChanges();
            }

            return output;
        }


        public Courses GetByIdCourseSimple(long id)
        {
            return _context.Courses.Where(x => x.id == id)
                                    .Include(a => a.CourseLevel)
                                    .Include(b => b.CourseCategory)
                                    .Include(c => c.CourseType)
                                    .Include(d => d.CourseOutline)
                                    .Include(e => e.CourseOutcome)
                                    .Include(f => f.CourseCompetencies)
                                        .ThenInclude(f => f.CourseCompetenciesCertificate)
                                    .Include(g => g.RelatedCourse)
                                        .ThenInclude(g => g.courseRelated)
                                    .Include(h => h.CourseLanguage)
                                    .Include(i => i.CourseTag)
                                    .Include(j => j.CourseInstructor)
                                    .FirstOrDefault();
        }

        public Courses GetByName(string title)
        {
            return _context.Courses.Where(x => x.title == title).FirstOrDefault();
        }
        public Courses GetByCode(string code)
        {
            return _context.Courses.Where(x => x.code == code).FirstOrDefault();
        }



        public bool DuplicateCourse(long id, string refCode, string title)
        {

            string sqlCourse = String.Format("INSERT INTO [course].[courses] (code, title, description, status, featureImage, featureVideo, isPublished, isVisible, durationTime, durationType, passingGrade, capacity, notifyInstructor, lmsProfile, publishDescription, createdAt, updatedAt) " +
                                          "SELECT CONCAT('','','{1}') code, CONCAT('', '', '{2}') title, description, status, featureImage, featureVideo, isPublished, isVisible, durationTime, durationType, passingGrade, capacity, notifyInstructor, lmsProfile, publishDescription, createdAt, updatedAt FROM [course].[courses] WHERE id = {0}", id, refCode, title);
            _context.Database.ExecuteSqlCommand(sqlCourse);

            var insertedCourse = GetByCode(refCode);


            if (insertedCourse.featureImage != null)
            {
                var updateFeatureImage = _context.Courses.Where(x => x.id == insertedCourse.id).FirstOrDefault();
                var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                string courseImageFolder = String.Format("{0}\\Content\\Images\\Course", path);

                if (!Directory.Exists(courseImageFolder))
                    Directory.CreateDirectory(courseImageFolder);

                var fileId = Guid.NewGuid();
                var extension = Path.GetExtension(insertedCourse.featureImage);
                var fileName = fileId.ToString() + extension.ToString().ToLower();

                System.IO.File.Copy(Path.Combine(courseImageFolder, insertedCourse.featureImage), Path.Combine(courseImageFolder, fileName), true);

                updateFeatureImage.featureImage = fileName;
                _context.SaveChanges();
            }

            if (insertedCourse.featureVideo != null)
            {
                var updateFeatureVideo = _context.Courses.Where(x => x.id == insertedCourse.id).FirstOrDefault();
                var path = Path.Combine(_hostingEnvironment.WebRootPath, _fileDirectory.virtualDirectory);
                string courseVideoFolder = String.Format("{0}\\Content\\Video\\Course", path);

                if (!Directory.Exists(courseVideoFolder))
                    Directory.CreateDirectory(courseVideoFolder);

                var fileIdVideo = Guid.NewGuid();
                var extension = Path.GetExtension(insertedCourse.featureVideo);
                var fileNameVideo = fileIdVideo.ToString() + extension.ToString().ToLower();

                System.IO.File.Copy(Path.Combine(courseVideoFolder, insertedCourse.featureVideo), Path.Combine(courseVideoFolder, fileNameVideo), true);

                updateFeatureVideo.featureVideo = fileNameVideo;
                _context.SaveChanges();
            }



            string sqlCourseLevel = String.Format("INSERT INTO [course].[course_level] (courseId, levelId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, levelId, createdAt, updatedAt FROM [course].[course_level] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseLevel);

            string sqlCourseCategory = String.Format("INSERT INTO [course].[course_category] (courseId, categoryId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, categoryId, createdAt, updatedAt FROM [course].[course_category] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseCategory);

            string sqlCourseType = String.Format("INSERT INTO [course].[course_type] (courseId, courseTypeId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, courseTypeId, createdAt, updatedAt FROM [course].[course_type] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseType);

            string sqlCourseLanguage = String.Format("INSERT INTO [course].[course_language] (courseId, languageId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, languageId, createdAt, updatedAt FROM [course].[course_language] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseLanguage);

            string sqlCourseTag = String.Format("INSERT INTO [course].[course_tags] (courseId, tagId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, tagId, createdAt, updatedAt FROM [course].[course_tags] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseTag);

            string sqlCourseInstructor = String.Format("INSERT INTO [course].[course_instructor] (courseId, userId, userGroupId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, userId, userGroupId, createdAt, updatedAt FROM [course].[course_instructor] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCourseInstructor);

            string sqlCoursePrerequisite = String.Format("INSERT INTO [course].[course_prerequisite] (courseId, preRequisiteId, createdAt, updatedAt) " +
                                                  "SELECT CONCAT('','','{0}') AS courseId, preRequisiteId, createdAt, updatedAt FROM [course].[course_prerequisite] WHERE courseId = {1}", insertedCourse.id, id);
            _context.Database.ExecuteSqlCommand(sqlCoursePrerequisite);

            string _sql = String.Format("SELECT a.* FROM [course].[course_related] AS A " +
                                        "LEFT JOIN[course].[course_related_list] as b ON b.courseRelatedId = a.id " +
                                        "WHERE a.courseId = {0}", id);
            var crList = _context.CourseRelated.FromSqlRaw(_sql)
                                            .Where(x => x.courseId == id)
                                            .Include(x => x.courseRelated)
                                            .Select(i => new
                                            {
                                                id = i.id,
                                                courseId = i.courseId,
                                                isPrerequisite = i.isPrerequisite,
                                                createdAt = i.createdAt,
                                                updatedAt = i.updatedAt,
                                                courseRelatedListCourseRelatedId = i.courseRelated.courseRelatedId,
                                                courseRelatedListCourseId = i.courseRelated.courseId,
                                            })
                                            .ToList();

            for (int x = 0; x < crList.Count(); x++)
            {

                CourseRelatedDetails crdModel = new CourseRelatedDetails();
                crdModel.courseId = insertedCourse.id;
                crdModel.isPrerequisite = crList[x].isPrerequisite;
                _context.CourseRelated.Add(crdModel);
                _context.SaveChanges();

                CourseRelatedList crlModel = new CourseRelatedList();
                crlModel.courseRelatedId = crdModel.id;
                crlModel.courseId = crList[x].courseRelatedListCourseId;
                _context.CourseRelatedList.Add(crlModel);
                _context.SaveChanges();

            }

            return true;
        }



        private bool isExists(Courses request)
        {
            return _context.Courses.Where(x => x.title == request.title).Any();
        }

        private bool isExistsById(long id, Courses request)
        {
            return _context.Courses.Where(x => x.title == request.title && x.id != id).Any();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

    }
}
