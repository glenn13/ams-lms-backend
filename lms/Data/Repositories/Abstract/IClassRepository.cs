using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface IClassRepository
    {
        object GetAll(long userId = 0);
    }
}
