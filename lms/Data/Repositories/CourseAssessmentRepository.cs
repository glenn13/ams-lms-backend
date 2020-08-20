using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class CourseAssessmentRepository : ICourseAssessmentRepository
    {
        private readonly lmsContext _context;

        public CourseAssessmentRepository (lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<CourseAssessment> GetAll()
        {
            return _context.CourseAssessment.Include(a => a.AssessmentType)
                                            .Include(b => b.UserGroup)
                                            .Include(c => c.CourseAssessmentItem)
                                            .ToList();
        }

        public IEnumerable<CourseAssessment> GetAllByCourse(long courseId)
        {
            return _context.CourseAssessment.Include(x => x.AssessmentType)
                                            .Include(x => x.UserGroup)
                                            .Include(x => x.CourseAssessmentItem)
                                            .Where(x => x.courseId == courseId)
                                            .ToList();
        }



        public bool Add (CourseAssessment request)
        {
            var output = false;
            var validate = isExists(request);
            if (validate == false)
            {

                CourseAssessment courseAssessment = new CourseAssessment();
                courseAssessment.courseId = request.courseId;
                courseAssessment.title = request.title;

                courseAssessment.userGroupId = request.userGroupId;
                courseAssessment.passingGrade = request.passingGrade;
                courseAssessment.isImmediate = request.isImmediate;

                //  Standard Entries: Assessment Type
                //  Defaults: 1 = Assignment; 2 = Exam; 3 = Quiz
                courseAssessment.assessmentTypeId = request.assessmentTypeId;

                if (request.isImmediate == 0)
                {
                    courseAssessment.fromDate = request.fromDate;
                    courseAssessment.toDate = request.toDate;
                }
                courseAssessment.isAttempts = request.isAttempts;
                if (request.isAttempts == 1)
                {
                    courseAssessment.attempts = request.attempts;
                }

                //  Static Entries: Based Type
                //  Default: 0 = No Limit; 1 = Exam-Based; 2 = Question-Based
                courseAssessment.basedType = request.basedType;
                if (request.basedType != 0)
                {
                    courseAssessment.duration = request.duration;
                }
                courseAssessment.isShuffle = request.isShuffle;
                courseAssessment.createdAt = DateTime.Now;
                courseAssessment.updatedAt = DateTime.Now;
                _context.CourseAssessment.Add(courseAssessment);
                _context.SaveChanges();
                AddSection(courseAssessment.id, request);
                output = true;
            }

            return output;
        }

        public int Update(long id, CourseAssessment request)
        {

            var model = GetById(id);
            var validate = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (validate == true)
                return 1;

            model.title = request.title;

            model.userGroupId = request.userGroupId;
            model.passingGrade = request.passingGrade;
            model.isImmediate = request.isImmediate;

            //  Standard Entries: Assessment Type
            //  Defaults: 1 = Assignment; 2 = Exam; 3 = Quiz
            model.assessmentTypeId = request.assessmentTypeId;

            if (request.isImmediate == 0)
            {
                model.fromDate = request.fromDate;
                model.toDate = request.toDate;
            }
            else
            {
                model.fromDate = null;
                model.toDate = null;
            }
            model.isAttempts = request.isAttempts;
            if (request.isAttempts == 1)
            {
                model.attempts = request.attempts;
            }
            else
            {
                model.attempts = 0;
            }

            //  Static Entries: Based Type
            //  Default: 0 = No Limit; 1 = Exam-Based; 2 = Question-Based
            model.basedType = request.basedType;
            if (request.basedType != 0)
            {
                model.duration = request.duration;
            }
            else
            {
                model.duration = null;
            }
            model.isShuffle = request.isShuffle;
            model.updatedAt = DateTime.Now;
            Save();

            AddSection(id, request);

            return 2;
        }

        public string Delete(long id)
        {
            var model = _context.CourseAssessment.Where(x => x.id == id)
                                                 .Include(a => a.CourseAssessmentItem)
                                                 .ThenInclude(b => b.CourseAssessmentItemChoices)
                                                 .FirstOrDefault();
            var counterLearnerAssessment = _context.LearnerCourseAssessment.Where(x => x.courseAssessmentId == id).Count();
            if (model == null)
                return "not exists";

            if (counterLearnerAssessment > 0)
                return "in used";

            _context.CourseAssessment.Remove(model);
            Save();
            return "deleted";
        }


        public bool DeleteItem(long id)
        {
            var output = false;
            var model = _context.CourseAssessmentItem.Where(x => x.id == id)
                                                     .Include(a => a.CourseAssessmentItemChoices)
                                                     .FirstOrDefault();
            if (model != null)
            {
                _context.CourseAssessmentItem.Remove(model);
                _context.SaveChanges();
                output = true;
            }
            return output;
        }


        public bool DeleteItemChoices(long id)
        {
            var output = false;
            var model = _context.CourseAssessmentItemChoices.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseAssessmentItemChoices.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }


        public bool AddSection (long id, CourseAssessment request)
        {


            //  Process of adding course assessment items
            foreach (CourseAssessmentItem caiVal in request.CourseAssessmentItem)
            {
                CourseAssessmentItem cai = new CourseAssessmentItem();
                cai.courseAssessmentid = id;
                cai.name = caiVal.name;

                //  Static Entries: Based Type
                //  Default: 0 = No Limit; 1 = Exam-Based; 2 = Question-Based
                //  if based type is Question-Based
                if (request.basedType == 2)
                {
                    cai.duration = caiVal.duration;
                }

                //  if assessment item type is not Essay
                //  Defaults: 1 = Essay; 2 = Multiple Choices; 3 = True / False
                cai.assessmentItemTypeId = caiVal.assessmentItemTypeId;
                if (caiVal.assessmentItemTypeId == 2)
                {
                    cai.isShuffle = caiVal.isShuffle;
                }
                else
                {
                    cai.minLength = caiVal.minLength;
                    cai.maxLength = caiVal.maxLength;
                }

                //  Static Entries: Based Type
                //  if assessment item type is True or False
                //  Defaults: 1 = Essay; 2 = Multiple Choices; 3 = True / False
                if (caiVal.assessmentItemTypeId == 3)
                {
                    cai.isTrue = caiVal.isTrue;
                    cai.isFalse = caiVal.isFalse;
                }
                cai.createdAt = DateTime.Now;
                cai.updatedAt = DateTime.Now;
                _context.CourseAssessmentItem.Add(cai);
                _context.SaveChanges();


                // Process assessment item choices if selected assessment item type is Multiple Choices
                if (caiVal.assessmentItemTypeId == 2)
                {
                    foreach (CourseAssessmentItemChoices caicVal in caiVal.CourseAssessmentItemChoices)
                    {
                        CourseAssessmentItemChoices caic = new CourseAssessmentItemChoices();
                        caic.courseAssessmentItemId = cai.id;
                        caic.name = caicVal.name;
                        caic.isCorrect = caicVal.isCorrect;
                        caic.createdAt = DateTime.Now;
                        caic.updatedAt = DateTime.Now;
                        _context.CourseAssessmentItemChoices.Add(caic);
                        _context.SaveChanges();
                    }
                }
            }
            return true;
        }


        public CourseAssessment GetById(long id)
        {

            return _context.CourseAssessment.Where(x => x.id == id)
                                                  .Include(a => a.AssessmentType)
                                                  .Include(b => b.UserGroup)
                                                  .Include(c => c.CourseAssessmentItem)
                                                    .ThenInclude(d => d.AssessmentItemType)
                                                  .Include(c => c.CourseAssessmentItem)
                                                    .ThenInclude(e => e.CourseAssessmentItemChoices)
                                                  .FirstOrDefault();
        }

        public IEnumerable<CourseAssessment> GetByCourseId(long id)
        {
            return _context.CourseAssessment.Where(x => x.courseId == id)
                                            .Include(c => c.CourseAssessmentItem)
                                                .ThenInclude(e => e.CourseAssessmentItemChoices)
                                            .ToList();
        }





        public bool DuplicateByCourseId(long id, long newCourseId)
        {

            var courseAssessment = GetByCourseId(id);

            foreach (CourseAssessment ca in courseAssessment)
            {
                CourseAssessment caModel = new CourseAssessment();
                caModel.courseId = newCourseId;
                caModel.title = ca.title;
                caModel.assessmentTypeId = ca.assessmentTypeId;
                caModel.userGroupId = ca.userGroupId;
                caModel.passingGrade = ca.passingGrade;
                caModel.isImmediate = ca.isImmediate;
                caModel.fromDate = ca.fromDate;
                caModel.toDate = ca.toDate;
                caModel.duration = ca.duration;
                caModel.isAttempts = ca.isAttempts;
                caModel.attempts = ca.attempts;
                caModel.basedType = ca.basedType;
                caModel.isShuffle = ca.isShuffle;
                caModel.createdAt = DateTime.Now;
                caModel.updatedAt = DateTime.Now;
                _context.CourseAssessment.Add(caModel);
                _context.SaveChanges();

                var courseAssessmentId = caModel.id;
                foreach (CourseAssessmentItem cai in ca.CourseAssessmentItem)
                {
                    CourseAssessmentItem caiModel = new CourseAssessmentItem();
                    caiModel.courseAssessmentid = caModel.id;
                    caiModel.name = cai.name;
                    caiModel.duration = cai.duration;
                    caiModel.assessmentItemTypeId = cai.assessmentItemTypeId;
                    caiModel.isShuffle = cai.isShuffle;
                    caiModel.minLength = cai.minLength;
                    caiModel.maxLength = cai.maxLength;
                    caiModel.isTrue = cai.isTrue;
                    caiModel.isFalse = cai.isFalse;
                    caiModel.createdAt = DateTime.Now;
                    caiModel.updatedAt = DateTime.Now;

                    _context.CourseAssessmentItem.Add(caiModel);
                    _context.SaveChanges();

                    foreach (CourseAssessmentItemChoices caic in cai.CourseAssessmentItemChoices)
                    {
                        CourseAssessmentItemChoices caicModel = new CourseAssessmentItemChoices();
                        caicModel.courseAssessmentItemId = caiModel.id;
                        caicModel.name = caic.name;
                        caicModel.isCorrect = caic.isCorrect;
                        _context.CourseAssessmentItemChoices.Add(caicModel);
                        _context.SaveChanges();
                    }
                }
            }
            return true;
        }



        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }



        private bool isExists(CourseAssessment request)
        {
            return _context.CourseAssessment.Where(x => x.title == request.title && x.assessmentTypeId == request.assessmentTypeId && x.userGroupId == request.userGroupId).Any();
        }
        private bool isExistsById(long id, CourseAssessment request)
        {
            return _context.CourseAssessment.Where(x => x.title == request.title && x.assessmentTypeId == request.assessmentTypeId && x.userGroupId == request.userGroupId && x.id != id).Any();
        }
    }
}
