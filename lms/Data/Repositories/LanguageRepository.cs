using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly lmsContext _context;
        public LanguageRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Language> GetAll()
        {
            return _context.Language.ToList();
        }

        public bool Add(Language request)
        {

            var output = false;
            var validate = _context.Language.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Language.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Language GetById(long id)
        {
            return _context.Language.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Language request)
        {

            Language model = GetById(id);
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
            var model = _context.Language.Where(x => x.id == id).FirstOrDefault();
            var model2 = _context.CourseLanguage.Where(x => x.languageId == id).Count();
            if (model == null)
                return "not exists";

            if (model2 > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Language.Remove(model);
            Save();
            return "deleted";
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Language request)
        {
            return _context.Language.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
