using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;
using System.Collections;

namespace lms.Data.Repositories
{ 
    public class ClassRepository : IClassRepository
    { 
        private readonly lmsContext _context;
        public ClassRepository (lmsContext context)
        {
            _context = context;
        }
        public object GetAll(long userId = 0)
        {

            return _context.CourseInstructor.Include(x => x.Course)
                                                .ThenInclude(x => x.Learner)
                                            .Where(x => x.userId == userId)
                                            .ToList();
        }


    }
}
