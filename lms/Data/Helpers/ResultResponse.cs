using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;
using Newtonsoft.Json;

namespace lms.Data.Helpers
{

    public class GenericResult
    {
        public bool Response { get; set; }
        public string Message { get; set; }
    }
    public class GenericResultId
    {
        public bool Response { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
    }

    public class RequestValidationModel
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public string Message { get; set; }
    }



    public class RequiredFields
    {
        public Courses Courses { get; set; }
        public CourseSession CourseSession { get; set; }
        public CourseOutline CourseOutline { get; set; }
        public CourseOutlineMedia CourseOutlineMedia { get; set; }
        public CourseOutlineMilestone CourseOutlineMilestone { get; set; }
        public CourseOutlinePrerequisite CourseOutlinePrerequisite { get; set; }
        public CourseOutcome CourseOutcome { get; set; }  
        public CourseAssessment CourseAssessment { get; set; } 
        public CourseInstructor Instructor { get; set; }
        public CourseCompetencies Competencies { get; set; }
        public CourseEvaluation CourseEvaluation { get; set; }
        //public Enrollment Enrollment { get; set; }
        public UserGroups UserGroups { get; set; }
        public Groups Groups { get; set; }
        public Status Status { get; set; }
        public Tags Tags { get; set; }
        public SessionType SessionType { get; set; }
        public Location Location { get; set; }
        public Department Department { get; set; }
        public Category Category { get; set; }
        public LearnerCourseOutline LearnerCourseOutline { get; set; }
        public LearnerCourseAssessment LearnerCourseAssessment { get; set; }
        public LearnerAppraisal LearnerAppraisal { get; set; }
        public Learner LearnerAppraisalFilter { get; set; }
        public Appraisal Appraisal { get; set; }
        public LearnerCourseAssessmentReminder LearnerCourseAssessmentReminder { get; set; }
        public Language Language { get; set; }
        public Level Level { get; set; }
        public Types Types { get; set; }
    }
}
 