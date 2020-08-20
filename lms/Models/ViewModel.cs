using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.Models
{
    public class LearnerEnrollment
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public long learnerId { get; set; }
        public long enrollmentType { get; set; }
        public long learnerStatusId { get; set; }
        public DateTime? learnerStartDate { get; set; }
        public long enrollmentId { get; set; }
        public long sessionId { get; set; }
        public long courseId { get; set; }
        public string courseTitle { get; set; }
        public long sessionTypeId { get; set; }
        public string sessionLocation { get; set; }
        public long userGroupId { get; set; }
        public int capacity { get; set; }
        public DateTime? sessionStartDate { get; set; }
        public DateTime? sesseionEndDate { get; set; }
        public string duration { get; set; }
        public long instructorId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class InstructorClass
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public long learnerId { get; set; }
        public long enrollmentType { get; set; }
        public long learnerStatusId { get; set; }
        public DateTime? learnerStartDate { get; set; }
        public long enrollmentId { get; set; }
        public long sessionId { get; set; }
        public long courseId { get; set; }
        public string courseTitle { get; set; }
        public long sessionTypeId { get; set; }
        public string sessionLocation { get; set; }
        public long userGroupId { get; set; }
        public int capacity { get; set; }
        public DateTime? sessionStartDate { get; set; }
        public DateTime? sesseionEndDate { get; set; }
        public string duration { get; set; }
        public long instructorId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public virtual IList<Groups> groups { get; set; }
    }

    public class VMLEarnerDashboard
    {
        //public long userId { get; set; }
        public virtual VMUser User { get; set; }
    }

    public class VMUser
    {
        public long userId { get; set; }
        public string empId { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
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
        public DateTime? dateApproved { get; set; }
        public DateTime? hireDate { get; set; }
        public DateTime? lastWorkingDate { get; set; }
    }


    public class VMModel
    {
        public virtual IList<Courses> Courses { get; set; }
        public virtual IList<Learner> Learner { get; set; }
        //public CourseSession CourseSession { get; set; }
        //public CourseOutline CourseOutline { get; set; }
        //public CourseOutlineMilestone CourseOutlineMilestone { get; set; }
        //public CourseOutlinePrerequisite CourseOutlinePrerequisite { get; set; }
        //public CourseOutcome CourseOutcome { get; set; }
        //public CourseAssessment CourseAssessment { get; set; }
        //public CourseInstructor Instructor { get; set; }
        //public CourseCompetencies Competencies { get; set; }
        //public CourseEvaluation CourseEvaluation { get; set; }
        ////public Enrollment Enrollment { get; set; }
        //public UserGroups UserGroups { get; set; }
        //public Groups Groups { get; set; }
        //public Status Status { get; set; }
        //public Tags Tags { get; set; }
        //public SessionType SessionType { get; set; }
        //public Location Location { get; set; }
        //public Department Department { get; set; }
        //public Category Category { get; set; }
        //public LearnerCourseOutline LearnerCourseOutline { get; set; }
        //public LearnerCourseAssessment LearnerCourseAssessment { get; set; }
        //public LearnerAppraisal LearnerAppraisal { get; set; }
        //public Learner LearnerAppraisalFilter { get; set; }
        //public Appraisal Appraisal { get; set; }
        //public LearnerCourseAssessmentReminder LearnerCourseAssessmentReminder { get; set; }
    }



    public class ClaimsDetails
    {
        public long id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public bool isActive { get; set; }
        public bool canCreate { get; set; }
        public bool canModify { get; set; }
        public bool canRemove { get; set; }
    }



    public class CourseRequest
    {

        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long status { get; set; }
        public IFormFile featureImage { get; set; }
        public IFormFile featureVideo { get; set; }
        public byte isPublished { get; set; }
        public byte isVisible { get; set; }
        public int durationTime { get; set; }
        public string durationType { get; set; }

        public int passingGrade { get; set; }
        public int capacity { get; set; }
        public IList<CourseLevel> CourseLevel { get; set; }
        public IList<CourseCategory> CourseCategory { get; set; }
        public IList<CourseType> CourseType { get; set; }
        public IList<CourseRelatedDetails> RelatedCourse { get; set; }
        public IList<CourseLanguage> CourseLanguage { get; set; }
        public IList<CourseTag> CourseTag { get; set; }
    }

    public class CourseOutlineRequest
    {
        public string title { get; set; }
        public long courseId { get; set; }
        public long userGroupId { get; set; }
        public byte visibility { get; set; }
        public IFormFile featureImage { get; set; }
        public IFormFile interactiveVideo { get; set; }
        public int duration { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual IList<CourseOutlinePrerequisite> CourseOutlinePrerequisite { get; set; }
        public virtual IList<CourseOutlineMedia> CourseOutlineMedia { get; set; }
        public virtual IList<CourseOutlineMilestoneRequest> CourseOutlineMilestone { get; set; }
        public virtual IList<LearnerCourseOutline> LearnerCourseOutline { get; set; }
        public IList<IFormFile> CourseOutlineMilestoneResourceFile { get; set; }
        public IList<IFormFile> CourseOutlineMediaFile { get; set; }
    }


    public class CourseOutlineMilestoneRequest
    {
        public long courseId { get; set; }
        public long courseOutlineId { get; set; }
        public string name { get; set; }
        public int lessonCompleted { get; set; }
        public IFormFile resourceFile { get; set; }
    }



    public class CourseCompetenciesRequest
    {
        public long courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long userGroupId { get; set; }
        public int lessonCompleted { get; set; }
        public int milestonesReached { get; set; }
        public int assessmentsSubmitted { get; set; }
        public int final { get; set; }
        public IList<IFormFile> files { get; set; }
    }

    public class FileInputModelCourseMilestone
    {
        public string name { get; set; }
        public int lessonCompleted { get; set; }
        public IFormFile files { get; set; }
    }
}
