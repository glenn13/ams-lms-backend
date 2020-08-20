using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public class CourseEvaluationRepository : ICourseEvaluationRepository
    {
        private readonly lmsContext _context;

        public CourseEvaluationRepository (lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<CourseEvaluation> GetAll()
        {
            return _context.CourseEvaluation.Include(x => x.UserGroup)
                                            .Include(x => x.EvaluationType)
                                            .Include(x => x.EvaluationAction)
                                            .Include(x => x.CourseEvaluationValues)
                                            .ToList();
        }

        public IEnumerable<CourseEvaluation> GetAllByCourse(long courseId)
        {
            return _context.CourseEvaluation.Include(x => x.UserGroup)
                                            .Include(x => x.EvaluationType)
                                            .Include(x => x.EvaluationAction)
                                            .Include(x => x.CourseEvaluationValues)
                                            .Where(x => x.courseId == courseId)
                                            .ToList();
        }

        public bool Add (CourseEvaluation request)
        {
            var output = false;
            var validate = isExists(request);
            if (validate == false)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;


                //  Will Appear if evaluationTypeId is 2
                //  Default: 1 = Rating; 2 = Single Answer; 3 = Comment
                if (request.evaluationTypeId != 2)
                    request.CourseEvaluationValues = null;

                _context.CourseEvaluation.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public CourseEvaluation GetById(long id)
        {
            return _context.CourseEvaluation.Where(x => x.id == id)
                                            .Include(x => x.UserGroup)
                                            .Include(x => x.EvaluationType)
                                            .Include(x => x.EvaluationAction)
                                            .Include(x => x.CourseEvaluationValues)
                                            .FirstOrDefault();
        }

        public int Update(long id, CourseEvaluation request)
        {
            CourseEvaluation model = GetById(id);
            var validate = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (validate != false)
                return 1;


            //  Will Appear if evaluationTypeId is 2
            //  Default: 1 = Rating; 2 = Single Answer; 3 = Comment
            if (request.evaluationTypeId != 2)
                request.CourseEvaluationValues = null;

            //_context.CourseEvaluation.Update(request);
            //Save();

            model.title = request.title;
            model.userGroupId = request.userGroupId;
            model.evaluationTypeId = request.evaluationTypeId;
            model.evaluationActionId = request.evaluationActionId;
            model.updatedAt = DateTime.Now;
            Save();

            // Insert Evaluation Values if evaluationTypeId is 2
            //  Default: 1 = Rating; 2 = Single Answer; 3 = Comment
            if (request.evaluationTypeId == 2)
            {
                foreach (CourseEvaluationValues x in request.CourseEvaluationValues)
                {
                    x.courseEvaluationId = id;
                    x.name = x.name;
                    x.createdAt = DateTime.Now;
                    x.updatedAt = DateTime.Now;
                    _context.CourseEvaluationValues.Add(x);
                    _context.SaveChanges();
                }
            }

            return 2;
        }


        public bool Delete(long id)
        {

            var output = false;
            var model = _context.CourseEvaluation.Where(x => x.id == id).Include(x => x.CourseEvaluationValues).FirstOrDefault();
            if (model != null)
            {
                _context.CourseEvaluation.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }


        public bool DeleteValues(long id)
        {

            var output = false;
            var model = _context.CourseEvaluationValues.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.CourseEvaluationValues.Remove(model);
                _context.SaveChanges();

                output = true;
            }
            return output;
        }



        public IEnumerable<CourseEvaluation> GetByCourseId(long id)
        {
            return _context.CourseEvaluation.Where(x => x.courseId == id)
                                            .Include(x => x.CourseEvaluationValues)
                                            .ToList();
        }



        public bool DuplicateByCourseId(long id, long newCourseId)
        {

            var courseEvaluation = GetByCourseId(id);

            foreach (CourseEvaluation ce in courseEvaluation)
            {
                CourseEvaluation ceModel = new CourseEvaluation();
                ceModel.courseId = newCourseId;
                ceModel.title = ce.title;
                ceModel.userGroupId = ce.userGroupId;
                ceModel.evaluationTypeId = ce.evaluationTypeId;
                ceModel.evaluationActionId = ce.evaluationActionId;
                ceModel.isRequired = ce.isRequired;
                ceModel.minValue = ce.minValue;
                ceModel.maxValue = ce.maxValue;
                ceModel.createdAt = DateTime.Now;
                ceModel.updatedAt = DateTime.Now;
                _context.CourseEvaluation.Add(ceModel);
                _context.SaveChanges();

                foreach (CourseEvaluationValues cev in ce.CourseEvaluationValues)
                {
                    CourseEvaluationValues cevModel = new CourseEvaluationValues();
                    cevModel.courseEvaluationId = ceModel.id;
                    cevModel.name = cev.name;
                    cevModel.createdAt = DateTime.Now;
                    cevModel.updatedAt = DateTime.Now;
                    _context.CourseEvaluationValues.Add(cevModel);
                    _context.SaveChanges();
                }
            }
            return true;
        }

        private bool isExists(CourseEvaluation request)
        {
            return _context.CourseEvaluation.Where(x => x.title == request.title && x.courseId == request.courseId && x.evaluationTypeId == request.evaluationTypeId).Any();
        }
        private bool isExistsById(long id, CourseEvaluation request)
        {
            return _context.CourseEvaluation.Where(x => x.title == request.title && x.userGroupId == request.userGroupId && x.evaluationTypeId == request.evaluationTypeId && x.id != id).Any();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
