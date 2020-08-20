using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;
using lms.Data.Helpers;

namespace lms.Data.Repositories
{
    public class LearnerRepository : ILearnerRepository
    {
        private readonly lmsContext _context; 

        public LearnerRepository(lmsContext context)
        {
            _context = context;
        }


        public int EnrollByInstructor(long courseId, long sessionId, VMEnrollment request)
        {

            var learnerIdList = new ArrayList();
            int noExistingUsers = 0;
            var courseNewStatus = _context.Status.Where(x => x.category == "Course" && x.name == "New").FirstOrDefault();
            if (request.Learner.Count() > 0)
            {
                foreach (Learner x in request.Learner)
                {
                    var checkExists = _context.Learner.Where(i => i.courseId == courseId && i.userId == x.userId).FirstOrDefault();
                    if (checkExists == null)
                    {
                        Learner e = new Learner();
                        e.courseId = courseId;
                        e.userId = x.userId;
                        e.enrollmentType = 1;   //  instructor enrolled
                        e.isNotify = x.isNotify;   //  instructor enrolled
                        e.statusId = courseNewStatus.id;
                        e.isApproved = 1;
                        _context.Learner.Add(e);
                        _context.SaveChanges();
                        learnerIdList.Add(e.id);


                        if (request.isAutoEnroll != 0)
                        {
                            foreach (LearnerSession vme in request.LearnerSession)
                            {

                                LearnerSession ls = new LearnerSession();
                                ls.courseId = courseId;
                                ls.learnerId = e.id;
                                ls.sessionId = sessionId;
                                ls.statusId = courseNewStatus.id;
                                ls.dateScheduled = vme.dateScheduled;
                                ls.createdAt = DateTime.Now;
                                ls.updatedAt = DateTime.Now;
                                _context.LearnerSession.Add(ls);
                                _context.SaveChanges();
                            }
                        }

                    }
                    else
                    {
                        noExistingUsers++;
                    }

                }
            }
            return noExistingUsers;
        }

        public bool UpdateLeanerEnrollment(long learnerId, long sessionId, Learner request)
        {
            var model = _context.Learner.Where(x => x.id == learnerId).FirstOrDefault();
            if (model != null)
            {
                model.isNotify = request.isNotify;
                model.notificationDetails = request.notificationDetails;
                _context.SaveChanges();

                var enrollmentStatus = _context.Status.Where(x => x.category == "Enrollment" && x.name == "New").FirstOrDefault();
                foreach (LearnerSession ls in request.LearnerSession)
                {
                    ls.courseId = model.courseId;
                    ls.learnerId = learnerId;
                    ls.sessionId = sessionId;
                    ls.statusId = enrollmentStatus.id;
                    ls.createdAt = DateTime.Now;
                    _context.LearnerSession.Add(ls);
                    _context.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(long learnerId)
        {
            var output = false;
            var model = _context.Learner.Where(x => x.id == learnerId).Include(x => x.LearnerSession).FirstOrDefault();
            if (model != null)
            {
                _context.Learner.RemoveRange(model);
                _context.SaveChanges();
                output = true;
            }
            return output;
        }

        public bool DeleteLearnerSession(long learnerSessionId)
        {
            var output = false;
            var model = _context.LearnerSession.Where(x => x.id == learnerSessionId).FirstOrDefault();
            if (model != null)
            {
                _context.LearnerSession.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }



        public bool AddCourseOutline(LearnerCourseOutline request)
        {
            var exists = isLearnerCourseOutlineExists(request);
            if (exists == true)
                return false;

            var courseOutline = _context.CourseOutline.Where(x => x.id == request.courseOutlineId)
                                                      .Include(x => x.Course)
                                                      .FirstOrDefault();
            if (courseOutline == null)
                return false;

            var passingGrade = courseOutline.Course.passingGrade;

            var status = new Status();
            if (request.score >= passingGrade)
                status = _context.Status.Where(x => x.category == "Course" && x.name == "Passed").FirstOrDefault();
            else
                status = _context.Status.Where(x => x.category == "Course" && x.name == "Failed").FirstOrDefault();

            request.statusId = status.id;
            request.courseStart = DateTime.Now;
            request.courseEnd = DateTime.Now;
            _context.LearnerCourseOutline.Add(request);
            _context.SaveChanges();

            return true;
        }

        public bool AddCourseAssessment(LearnerCourseAssessment request)
        {
            var limit = isLearnerAssessmentExceedLimit(request);
            if (limit == false)
            {
                return false;
            }
            else
            {
                request.dateTaken = DateTime.Now;
                _context.LearnerCourseAssessment.Add(request);
                _context.SaveChanges();

                return true;
            }
        }

        public bool AssessLearner(long learnerId, Learner request)
        {
            var output = false;
            var model = _context.Learner.Where(x => x.id == learnerId).Include(x => x.Course).FirstOrDefault();
            if (model != null)
            {
                var assessmentStatus = new Status();
                if (model.Course.passingGrade > request.finalScore)
                {
                    assessmentStatus = _context.Status.Where(x => x.category == "Assessment" && x.name == "Failed").FirstOrDefault();
                }
                else
                {
                    assessmentStatus = _context.Status.Where(x => x.category == "Assessment" && x.name == "New").FirstOrDefault();
                }
                model.assessmentStatusId = assessmentStatus.id;
                model.finalScore = request.finalScore;

                foreach (LearnerCourseAssessment i in request.LearnerCourseAssessment)
                {
                    var updateAssessment = _context.LearnerCourseAssessment.Where(x => x.id == i.id).FirstOrDefault();
                    updateAssessment.points = i.points;
                    _context.SaveChanges();
                }
                output = true;
            }
            return output;
        }


        public IEnumerable<Learner> GetLearnerCompetencies(long userId)
        {

            var output = _context.Learner.Where(x => x.userId == userId)
                                        .Include(x => x.Course)
                                            .ThenInclude(x => x.CourseCompetencies)
                                        .Include(x => x.Course.CourseOutline)
                                            .ThenInclude(x => x.LearnerCourseOutline)
                                        .Include(x => x.User)
                                        .ToList();
            ////foreach (Learner x in model)
            ////{
            ////    //int cou
            ////    var finalScore = x.finalScore;
            ////    var passingGrade = x.Course.passingGrade;
            ////    return Ok(finalScore + " " + passingGrade);
            ////}

            ////var lea
            //foreach (Learner x in model)
            //{
            //    int noOfCourseOutline = x.Course.CourseOutline.Count();
            //    int noOfLessonsCompleted = 0;
            //    CourseCompetencies compileCourseCompetencies = new CourseCompetencies();
            //    foreach (CourseOutline co in x.Course.CourseOutline)
            //    {
            //        if (co.LearnerCourseOutline.Count() > 0)
            //        {
            //            noOfLessonsCompleted++;
            //        }

            //    }
            //    foreach (CourseCompetencies cc in x.Course.CourseCompetencies)
            //    {
            //        if (noOfLessonsCompleted >= cc.lessonCompleted)
            //        {
            //            compileCourseCompetencies = typeof(Courses).IsAssignableFrom(cc);
            //        }
            //    }
            //    return Ok(compileCourseCompetencies);
            //    return Ok(noOfCourseOutline + " " + noOfLessonsCompleted);
            //}
            return output;

        }

        public IEnumerable<Learner> GetLearnerAssessment(long userId)
        {
            return _context.Learner.Where(x => x.userId == userId)
                                         .Include(x => x.Course)
                                         .Include(x => x.User)
                                         .ToList();
        }

        public IEnumerable<Learner> GetLearnerAppraisal(long userId)
        {
            return _context.Learner.Where(x => x.userId == userId)
                                   .Include(x => x.Course)
                                   .Include(x => x.LearnerAppraisal)
                                        .ThenInclude(x => x.Appraisal)
                                   .Include(x => x.Course.CourseInstructor)
                                   .Include(x => x.User)
                                   .ToList();
        }

        public object AddLearnerAppraisal(long learnerId, Learner request)
        {
            string result = "";
            int countDuplicate = 0;
            int countNoRecord = 0;
            bool isValidationError = true;
            var arr = new ArrayList();

            foreach (LearnerAppraisal x in request.LearnerAppraisal)
            {
                //if (isLearnerCourseAppraisalExists(learnerId, x) == true)
                //{
                //    countDuplicate++;
                //    //arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                //}

                if (x.courseId == 0 || x.appraisalId == 0)
                {
                    isValidationError = false;
                    if (x.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Answer for course appraisal " + x.recommendation + " dont have course assigned" });
                    else if (x.appraisalId == 0)
                        arr.Add(new RequestValidationModel { Name = "Appraisal ID", Parameter = "appraisalId", Message = "Answer for course appraisal " + x.recommendation + " dont have appraisal assigned" });
                }
                else
                {
                    if (x.id == 0)
                    {
                        if (isLearnerCourseAppraisalExists(learnerId, x) == true)
                        {
                            countDuplicate++;
                        }
                        else
                        {
                            LearnerAppraisal la = new LearnerAppraisal();
                            la.learnerId = learnerId;
                            la.courseId = x.courseId;
                            la.appraisalId = x.appraisalId;
                            la.recommendation = x.recommendation;
                            la.rating = x.rating;
                            la.createdAt = DateTime.Now;
                            _context.LearnerAppraisal.Add(la);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var la = _context.LearnerAppraisal.Where(i => i.id == x.id).FirstOrDefault();
                        if (la != null)
                        {
                            if (isLearnerCourseAppraisalExistsById(learnerId, x) == true)
                            {
                                countDuplicate++;
                            }
                            else
                            {
                                la.recommendation = x.recommendation;
                                la.rating = x.rating;
                                _context.SaveChanges();
                            }
                        } 
                        else
                        {
                            countNoRecord++;
                        }
                    }
                }
                
            }

            if (countDuplicate > 0)
                arr.Add(new GenericResult { Response = false, Message = "There are (" + countDuplicate + ") record that already exists" });
            else if (countNoRecord > 0)
                arr.Add(new GenericResult { Response = false, Message = "There are (" + countNoRecord + ") entry that is not valid due to to ID mismatch" });
            else if (isValidationError == true)
                arr.Add(new GenericResult { Response = true, Message = "Appraisal has been successfully saved" });
            
                
            return arr;

        }

        public bool ApproveLearner(long learnerId)
        {
            var output = _context.Learner.Where(x => x.id == learnerId).FirstOrDefault();
            if (output == null)
                return false;

            output.isApproved = 1;
            _context.SaveChanges();
            return true;
        }


        public bool AddCourseReview(long learnerId, Learner request)
        {
            var learner = _context.Learner.Where(x => x.id == learnerId).FirstOrDefault();
            if (learner == null)
                return false;

            learner.instructorRating = request.instructorRating;
            learner.courseRating = request.courseRating;
            learner.courseReview = request.courseReview;
            _context.SaveChanges();
            return true;
        }

        public object LearnerCourse(long userId)
        {
            return _context.Learner.Where(x => x.userId == userId)
                                         .Include(x => x.Course.CourseOutline)
                                            .ThenInclude(x => x.LearnerCourseOutline)
                                         .Include(x => x.Course.CourseTag)
                                            .ThenInclude(x => x.Tag)
                                         .Include(x => x.Course.CourseLanguage)
                                            .ThenInclude(x => x.language)
                                         .Include(x => x.Course.CourseType)
                                            .ThenInclude(x => x.courseType)
                                         .Include(x => x.Course.RelatedCourse)
                                            .ThenInclude(x => x.courseRelated.Course)
                                         .Select(x => new {
                                             x.id,
                                             x.courseId,
                                             x.userId,
                                             x.enrollmentType,
                                             x.statusId,
                                             statusName = x.Status.name,
                                             statusColor = x.Status.color,
                                             x.assessmentStatusId,
                                             x.isRecommendCourse,
                                             x.instructorRating,
                                             x.courseRating,
                                             x.courseReview,
                                             x.finalScore,
                                             x.totalHoursTaken,
                                             x.isNotify,
                                             x.isApproved,
                                             x.notificationDetails,
                                             x.startDate,
                                             x.endDate,
                                             x.appraisalDate,
                                             x.overallRating,
                                             courseCode = x.Course.code,
                                             courseTitle = x.Course.title,
                                             courseDescription = x.Course.description,
                                             courseFeatureImage = x.Course.featureImage,
                                             courseFeatureVideo = x.Course.featureVideo,
                                             courseDurationTime = x.Course.durationTime,
                                             courseDurationType = x.Course.durationType,
                                             coursePassingGrade = x.Course.passingGrade,
                                             courseCapacity = x.Course.capacity,
                                             courseType = x.Course.CourseType.Select(a => new
                                             {
                                                 a.id,
                                                 a.courseId,
                                                 a.courseTypeId,
                                                 a.courseType.name
                                             }),
                                             relatedCourse = x.Course.RelatedCourse.Select(b => new
                                             {
                                                 relatedCourseId = b.id,
                                                 b.courseId,
                                                 b.isPrerequisite,
                                                 courseRelatedId = b.courseRelated.id,
                                                 courseRelatedCourseId = b.courseId,
                                                 courseRaltedCourseCode = b.Course.code,
                                                 courseRaltedCourseTitle = b.Course.title,
                                                 courseRaltedCourseDescription = b.Course.description,
                                                 courseRaltedCourseFeatureImage = b.Course.featureImage,
                                                 courseRaltedCourseFeatureVideo = b.Course.featureVideo,
                                                 courseRaltedCourseDurationTime = b.Course.durationTime,
                                                 courseRaltedCourseDurationType = b.Course.durationType,
                                                 courseRaltedCoursePassingGrade = b.Course.passingGrade,
                                                 courseRaltedCourseCapacity = b.Course.capacity,
                                             }),
                                             courseLanguage = x.Course.CourseLanguage.Select(c => new
                                             {
                                                 c.id,
                                                 c.courseId,
                                                 languageId = c.languageId,
                                                 languageName = c.language.name
                                             }),
                                             courseTag = x.Course.CourseTag.Select(d => new
                                             {
                                                 d.id,
                                                 d.courseId,
                                                 d.tagId,
                                                 tagName = d.Tag.name
                                             }),
                                             courseOutline = x.Course.CourseOutline.Select(e => new
                                             {
                                                 e.id,
                                                 e.title,
                                                 e.courseId,
                                                 e.featureImage,
                                                 e.interactiveVideo,
                                                 e.duration,
                                                 e.description,
                                                 courseOutlinePrerequisite = e.CourseOutlinePrerequisite.Select(g => new
                                                 {
                                                     g.id,
                                                     g.courseId,
                                                     g.courseOutlineId,
                                                     g.preRequisiteId
                                                 }),
                                                 courseOutlineMedia = e.CourseOutlineMedia.Select(h => new
                                                 {
                                                     h.id,
                                                     h.courseId,
                                                     h.courseOutlineId,
                                                     h.resourceFile
                                                 }),
                                                 courseOutlineMilestone = e.CourseOutlineMilestone.Select(i => new
                                                 {
                                                     i.id,
                                                     i.courseId,
                                                     i.courseOutlineId,
                                                     i.name,
                                                     i.lessonCompleted,
                                                     i.resourceFile
                                                 }),
                                                 LearnerCourseOutline = e.LearnerCourseOutline.Where(e => e.Learner.userId == userId).Select(f => new
                                                 {
                                                     f.id,
                                                     f.courseOutlineId,
                                                     f.learnerId,
                                                     f.statusId,
                                                     statusName = f.Status.name,
                                                     f.courseStart,
                                                     f.courseEnd,
                                                     f.hoursTaken
                                                 })
                                             })
                                         })
                                         .ToList();
        }




        private bool isLearnerCourseAppraisalExists(long learnerId, LearnerAppraisal request)
        {
            return _context.LearnerAppraisal.Where(x => x.learnerId == learnerId && x.courseId == request.courseId && x.appraisalId == request.appraisalId).Any();
        }
        private bool isLearnerCourseAppraisalExistsById(long learnerId, LearnerAppraisal request)
        {
            return _context.LearnerAppraisal.Where(x => x.learnerId == request.learnerId && x.courseId == request.courseId && x.appraisalId == request.appraisalId && x.id != request.id).Any();
        }
        private bool isLearnerCourseOutlineExists(LearnerCourseOutline request)
        {
            return _context.LearnerCourseOutline.Where(x => x.courseOutlineId == request.courseOutlineId && x.learnerId == request.learnerId).Any();
        }

        private bool isLearnerAssessmentExceedLimit(LearnerCourseAssessment request)
        {
            var result = _context.CourseAssessment.Where(x => x.id == request.courseAssessmentId)
                                                  .Include(x => x.LearnerCourseAssessment)
                                                  .Select(i => new {
                                                      attempts = i.attempts,
                                                      NoOfRecords = i.LearnerCourseAssessment
                                                                                       .Where(o => o.learnerId == request.learnerId && o.courseAssessmentItemId == request.courseAssessmentItemId)
                                                                                       .Count()
                                                  })
                                                  .FirstOrDefault();
            if (result.attempts != 0)
            {
                if (result.NoOfRecords < result.attempts)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }

        }


    }
}
