using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class SessionTypeRepository : ISessionTypeRepository
    {
        private readonly lmsContext _context;
        public SessionTypeRepository (lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<SessionType> GetAll ()
        {
            return _context.SessionType.ToList();
        }

        public bool Add(SessionType request)
        {
            var output = false;
            var validate = isExists(request);
            if (validate == false)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                _context.SessionType.Add(request);
                Save();
                output = true;
            }

            return output;
        }

        public SessionType GetById(long id)
        {
            return _context.SessionType.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, SessionType request)
        {
            SessionType model = GetById(id);
            var validate = isExistsById(id, request);

            if (model == null)
                return 0;
            else if (validate == true)
                return 1;

            model.name = request.name;
            model.description = request.description;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }

        public string Delete(long id)
        {
            var model = _context.SessionType.Where(x => x.id == id).Include(x => x.CourseSession).FirstOrDefault();
            if (model == null)
                return "not exists";

            if (model.CourseSession.Count() > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.SessionType.Remove(model);
            Save();
            return "deleted";

        }

        public bool Save ()
        {
            return _context.SaveChanges() > 0;
        }



        private bool isExists(SessionType request)
        {
            return _context.SessionType.Where(x => x.name == request.name).Any();
        }

        private bool isExistsById(long id, SessionType request)
        {
            return _context.SessionType.Where(x => x.name == request.name && x.id != id).Any();
        }


    }
}
