using lms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace lms.Data.Repositories
{
    public class AppraisalRepository : IAppraisalRepository
    {
        private readonly lmsContext _context;
        public AppraisalRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Appraisal> GetAll()
        {
            return _context.Appraisal.Include(x => x.LearnerAppraisal).ToList();
        }

        public bool Add(Appraisal request)
        {

            var output = false;
            var validate = _context.Appraisal.Where(x => x.name == request.name && x.courseTypeId == request.courseTypeId)
                                             .FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Appraisal.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Appraisal GetById(long id)
        {
            return _context.Appraisal.Where(x => x.id == id).Include(x => x.CourseType).FirstOrDefault();
        }

        public int Update(long id, Appraisal request)
        {

            Appraisal model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            model.courseTypeId = request.courseTypeId;
            Save();
            return 2;
        }

        public string Delete(long id)
        {

            var model = _context.Appraisal.Where(x => x.id == id).Include(x => x.LearnerAppraisal).FirstOrDefault();
            if (model == null)
                return "not exists";

            if (model.LearnerAppraisal.Count() > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Appraisal.Remove(model);
            Save();
            return "deleted";

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Appraisal request)
        {
            return _context.Appraisal.Where(x => x.name == request.name && x.courseTypeId == request.courseTypeId && x.id != id).Any();
        }
    }
}
