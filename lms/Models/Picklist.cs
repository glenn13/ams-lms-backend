using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms.Models
{
    [Table("location", Schema = "picklist")]
    public class Location
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Location()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("department", Schema = "picklist")]
    public class Department
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public Department()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("session_type", Schema = "picklist")]
    public class SessionType
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual IList<CourseSession> CourseSession { get; set; }

        public SessionType()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_level", Schema = "picklist")]
    public class Level
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        //public IEnumerable<CourseLevel> CourseLevel { get; set; }
        public Level()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_category", Schema = "picklist")]
    public class Category
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        //public virtual IList<CourseCategory> CourseCategory { get; set; }
        public Category()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_type", Schema = "picklist")]
    public class Types
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        //public virtual IList<CourseType> CourseType { get; set; }
        public Types()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("language", Schema = "picklist")]
    public class Language
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        //public virtual IList<CourseLanguage> CourseLanguage { get; set; }
        public Language()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_prerequisite", Schema = "picklist")]
    public class CoursePrerequisite
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    [Table("tags", Schema = "picklist")]
    public class Tags
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        //public virtual IList<CourseTag> CourseTag { get; set; }
        public Tags()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("assessment_type", Schema = "picklist")]
    public class AssessmentType
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public AssessmentType()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("assessment_item_type", Schema = "picklist")]
    public class AssessmentItemType
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public AssessmentItemType()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("evaluation_type", Schema = "picklist")]
    public class EvaluationType
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public EvaluationType()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("evaluation_action", Schema = "picklist")]
    public class EvaluationAction
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public EvaluationAction()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("appraisal", Schema = "picklist")]
    public class Appraisal
    {
        public long id { get; set; }
        public long courseTypeId { get; set; }
        public string name { get; set; }
        public byte isEditable { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Types CourseType { get; set; }
        public virtual IList<LearnerAppraisal> LearnerAppraisal { get; set; }

        public Appraisal()
        {
            updatedAt = DateTime.Now;
        }
    }
}
