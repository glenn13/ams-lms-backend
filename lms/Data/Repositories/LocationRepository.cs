using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly lmsContext _context;
        public LocationRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Location> GetAll()
        {
            return _context.Location.ToList();
        }

        public bool Add(Location request)
        {

            var output = false;
            var validate = _context.Location.Where(x => x.name == request.name && x.code == request.code).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                var model = _context.Location.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Location GetById(long id)
        {
            return _context.Location.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Location request)
        {

            Location model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            model.code = request.code;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }

        public string Delete(long id)
        {
            var model = _context.Location.Where(x => x.id == id).FirstOrDefault();
            if (model == null)
                return "not exists";

            if (model.isEditable == 1)
                return "not editable";

            _context.Location.Remove(model);
            Save();
            return "deleted";

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Location request)
        {
            return _context.Location.Where(x => x.name == request.name && x.code == request.code && x.id != id).Any();
        }
    }
}
