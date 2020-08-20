using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly lmsContext _context;
        public TagsRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Tags> GetAll()
        {
            return _context.Tags.ToList();
        }

        public bool Add(Tags request)
        {

            var output = false;
            var validate = _context.Tags.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Tags.Add(request);
                Save();
                output = true;
            }
            return output;
        }
         
        public Tags GetById(long id)
        {
            return _context.Tags.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Tags request)
        {

            Tags model = GetById(id);
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
            var model = _context.Tags.Where(x => x.id == id).FirstOrDefault();
            var model2 = _context.CourseTag.Where(x => x.tagId == id).Count();
            if (model == null)
                return "not exists";

            if (model2 > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Tags.Remove(model);
            Save();
            return "deleted";

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Tags request)
        {
            return _context.Tags.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
