using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms.Models
{

    [Table("status", Schema = "settings")]
    public class Status
    {
        public long id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string color { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; } 
    }

    //[Table("audit_trail", Schema = "settings")]
}
