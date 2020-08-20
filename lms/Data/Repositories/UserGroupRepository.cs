using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly lmsContext _context;
        public UserGroupRepository(lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<UserGroups> GetAll()
        {
            return _context.UserGroups.Include(x => x.Groups)
                                        .ThenInclude(x => x.User)
                                      .ToList();
        }

        public bool Add(UserGroups request)
        {

            var output = false;
            var validate = _context.UserGroups.Where(x => x.name == request.name).FirstOrDefault();
            if (validate == null)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                var model = _context.UserGroups.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public UserGroups GetById(long id)
        {
            return _context.UserGroups.Where(x => x.id == id)
                                      .Include(x => x.Groups)
                                        .ThenInclude(x => x.User)
                                      .FirstOrDefault();
        }

        public int Update(long id, UserGroups request)
        {

            UserGroups model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.name = request.name;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }

        public bool Delete(long id)
        {
            var output = false;
            var model = _context.UserGroups.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.UserGroups.Remove(model);
                Save();

                output = true;
            }
            return output;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExistsById(long id, UserGroups request)
        {
            return _context.UserGroups.Where(x => x.name == request.name && x.id != id).Any();
        }
    }
}
