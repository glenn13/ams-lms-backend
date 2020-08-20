using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms.Models
{

    [Table("users", Schema = "settings")]
    public class Users
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string empId { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string email { get; set; }

        public long projId { get; set; }
        public long deptId { get; set; }
        public long positionId { get; set; }
        public long internationalStatusId { get; set; }
        public long obsId { get; set; }
        public long branchId { get; set; }
        public byte isActive { get; set; }
        public DateTime? birthday { get; set; }
        public byte gender { get; set; }
        public byte isInstructor { get; set; }
        public byte isAdministrator { get; set; }
        public byte isLearner { get; set; }
        public byte canCreate { get; set; }
        public byte canModify { get; set; }
        public byte canRemove { get; set; }
        public DateTime? dateApproved { get; set; }
        public DateTime? hireDate { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public string token { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual IList<Groups> Groups { get; set; }
        public virtual IList<Learner> Learner { get; set; }
        public static object Identity { get; internal set; }

        public Users()
        {
            updatedAt = DateTime.Now;
        }
    }


    [Table("user_groups", Schema = "settings")]
    public class UserGroups
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual IList<Groups> Groups { get; set; }

        public UserGroups()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("groups", Schema = "settings")]
    public class Groups
    {
        public long id { get; set; }
        public long userId { get; set; }
        public long userGroupId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual Users User { get; set; }
        public virtual UserGroups UserGroup { get; set; }

        public Groups()
        {
            updatedAt = DateTime.Now;
        }
    }
}
