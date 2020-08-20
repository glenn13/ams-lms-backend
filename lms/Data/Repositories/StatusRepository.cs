using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly lmsContext _context;
        public StatusRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Status> GetAll()
        {
            return _context.Status.ToList();
        }

        public bool Add(Status request)
        {

            var output = false;
            var validate = _context.Status.Where(x => x.name == request.name && x.category == request.category).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                var model = _context.Status.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Status GetById(long id)
        {
            return _context.Status.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Status request)
        {

            Status model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            model.name = request.name; 
            model.name = request.name;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }

        public bool Delete(long id)
        {
            var output = false;
            var model = _context.Status.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.Status.Remove(model);
                Save();

                output = true;
            }
            return output;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Status request)
        {
            return _context.Status.Where(x => x.name == request.name && x.category == request.category && x.id != id).Any();
        }
    }
}
