using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
  
namespace lms.Data.Repositories
{  
    public interface IEnrollmentRepository
    {
        object GetAll(long userId = 0);
        Learner GetById(long id);

        bool DeleteEnrollee(long id);
        bool Save();
    }
}
