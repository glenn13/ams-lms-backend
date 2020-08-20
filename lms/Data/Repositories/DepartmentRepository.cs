using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly lmsContext _context;
        public DepartmentRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Department.ToList();
        }

        public bool Add(Department request)
        {

            var output = false;
            var validate = _context.Department.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                _context.Department.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Department GetById(long id)
        {
            return _context.Department.Where(x => x.id == id).FirstOrDefault();
        }

        public int Update(long id, Department request)
        {

            Department model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            model.code = request.code;
            Save();
            return 2;
        }

        public string Delete(long id)
        {
            var model = _context.Department.Where(x => x.id == id).FirstOrDefault();
            var counter = _context.Users.Where(x => x.deptId == id).Count();
            if (model == null)
                return "not exists";

            if (counter > 0)
                return "in used";

            if (model.isEditable == 1)
                return "not editable";

            _context.Department.Remove(model);
            Save();
            return "deleted";

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, Department request)
        {
            return _context.Department.Where(x => x.name == request.name && x.code == request.code && x.id != id).Any();
        }
    }
}
