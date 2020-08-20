using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly lmsContext _context;
        public LevelRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Level> GetAll()
        {
            return _context.Level.ToList();
        }

        public bool Add(Level request)
        {

            var output = false;
            var validate = _context.Level.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Level.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Level GetById(long id)
        {
            return _context.Level.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Level request)
        {

            Level model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            Save();
            return 2;
        }

        public string Delete(long id)
        {
            var model = _context.Level.Where(x => x.id == id).FirstOrDefault();
            var model2 = _context.CourseLevel.Where(x => x.levelId == id).Count();
            if (model == null)
                return "not exists";

            if (model2 > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Level.Remove(model);
            Save();
            return "deleted";
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Level request)
        {
            return _context.Level.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
