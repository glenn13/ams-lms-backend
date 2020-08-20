using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.Data.Core
{
    public interface IFileDirectory
    {
        string url { get; }
        string virtualDirectory { get; }
    }

    public class FileDirectory : IFileDirectory
    {
        public string url { get; set; }
        public string virtualDirectory { get; set; }
    }
}
