using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data.Repositories
{
    public class GroupRepository : IGroupRepository
    {

        private readonly lmsContext _context;
        public GroupRepository (lmsContext context)
        {
            _context = context;
        }

        public IEnumerable<Groups> GetAll()
        {
            return _context.Groups.Include(x => x.User)
                                  .Include(x => x.UserGroup)
                                  .ToList();
        }

        public bool Add(Groups request)
        {

            var output = false;
            if (isExists(request) == false)
            {
                request.createdAt = DateTime.Now;
                request.updatedAt = DateTime.Now;
                _context.Groups.Add(request);
                Save();
                output = true;
            }
            return output;
        }

        public Groups GetById(long id)
        {
            return _context.Groups.Where(x => x.id == id)
                                  .Include(x => x.User)
                                  .Include(x => x.UserGroup)
                                  .FirstOrDefault();
        }

        public int Update(long id, Groups request)
        {

            Groups model = GetById(id);
            var exists = isExistsById(id, request);
            if (model == null)
                return 0;
            else if (exists == true)
                return 1;
            model.userId = request.userId;
            model.userGroupId = request.userGroupId;
            model.updatedAt = DateTime.Now;
            Save();
            return 2;
        }

        public bool Delete(long id)
        {
            var output = false;
            var model = _context.Groups.Where(x => x.id == id).FirstOrDefault();
            if (model != null)
            {
                _context.Groups.Remove(model);
                Save();

                output = true;
            }
            return output;
        }

        public IEnumerable<Groups> SearchByUserGroup(long id)
        {
            return _context.Groups.Where(x => x.userGroupId == id)
                                  .Include(x => x.User)
                                  .ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private bool isExists(Groups request)
        {
            return _context.Groups.Where(x => x.userId == request.userId && x.userGroupId == request.userGroupId).Any();
        }

        private bool isExistsById(long id, Groups request)
        {
            return _context.Groups.Where(x => x.userId == request.userId && x.userGroupId == request.userGroupId && x.id != id).Any();
        }
    }
}
