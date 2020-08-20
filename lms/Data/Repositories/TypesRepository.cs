using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class TypesRepository : ITypesRepository
    {
        private readonly lmsContext _context;
        public TypesRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Types> GetAll()
        {
            return _context.Types.ToList();
        }

        public bool Add(Types request)
        {

            var output = false;
            var validate = _context.Types.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Types.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Types GetById(long id)
        {
            return _context.Types.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Types request)
        {

            Types model = GetById(id);
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
            var model = _context.Types.Where(x => x.id == id).FirstOrDefault();
            var model2 = _context.CourseType.Where(x => x.courseTypeId == id).Count();
            if (model == null)
                return "not exists";

            if (model2 > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Types.Remove(model);
            Save();
            return "deleted";
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Types request)
        {
            return _context.Types.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
