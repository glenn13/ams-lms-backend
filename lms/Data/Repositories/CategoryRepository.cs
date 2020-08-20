using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly lmsContext _context;
        public CategoryRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Category.ToList();
        }

        public bool Add(Category request)
        {

            var output = false;
            var validate = _context.Category.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Category.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Category GetById(long id)
        {
            return _context.Category.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Category request)
        {

            Category model = GetById(id);
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
            var model = _context.Category.Where(x => x.id == id).FirstOrDefault();
            var model2 = _context.CourseCategory.Where(x => x.categoryId == id).Count();
            if (model == null)
                return "not exists";

            if (model2 > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Category.Remove(model);
            Save();
            return "deleted";
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Category request)
        {
            return _context.Category.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
