using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms.Models
{

    /// <summary>
    /// Course
    /// </summary>

    [Table("courses", Schema = "course")]
    public class Courses
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long status { get; set; }
        public string featureImage { get; set; }
        public string featureVideo { get; set; }
        public byte isPublished { get; set; }
        public byte requestForPublish { get; set; }
        public byte isVisible { get; set; }
        public int durationTime { get; set; }
        public string durationType { get; set; }

        public int passingGrade { get; set; }
        public int capacity { get; set; }
        public byte notifyInstructor { get; set; }
        public long lmsProfile { get; set; }
        public string publishDescription { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual IList<CourseLevel> CourseLevel { get; set; }
        public virtual IList<CourseOutcome> CourseOutcome { get; set; }
        public virtual IList<CourseCategory> CourseCategory { get; set; }
        public virtual IList<CourseCompetencies> CourseCompetencies { get; set; }
        public virtual IList<CourseType> CourseType { get; set; }
        public virtual IList<CourseRelatedDetails> RelatedCourse { get; set; }
        public virtual IList<CourseLanguage> CourseLanguage { get; set; }
        public virtual IList<CourseTag> CourseTag { get; set; }
        public virtual IList<CourseSession> Session { get; set; }
        public virtual IList<CourseOutline> CourseOutline { get; set; }
        public virtual IList<CourseInstructor> CourseInstructor { get; set; }
        public virtual IList<Learner> Learner { get; set; }
        public virtual IList<CourseOutlineMilestone> CourseOutlineMilestone { get; set; }
        public virtual IList<CourseOutlineMedia> CourseOutlineMedia { get; set; }
        public virtual IList<CourseOutlinePrerequisite> CourseOutlinePrerequisite { get; set; }
        public virtual IList<CourseAssessment> CourseAssessment { get; set; }

        public Courses()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_level", Schema = "course")]
    public class CourseLevel
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long levelId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual Level level { get; set; }
        public virtual Courses Course { get; set; }

        public CourseLevel()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_category", Schema = "course")]
    public class CourseCategory
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long categoryId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Category category { get; set; }
        public virtual Courses Course { get; set; }


        public CourseCategory()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }

    [Table("course_type", Schema = "course")]
    public class CourseType
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long courseTypeId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Types courseType { get; set; }
        public virtual Courses Course { get; set; }


        public CourseType()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_related", Schema = "course")]
    public class CourseRelatedDetails
    {
        public long id { get; set; }
        public long courseId { get; set; }
        [NotMapped]
        public long relatedCourseId { get; set; }
        public byte isPrerequisite { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }
        public virtual CourseRelatedList courseRelated { get; set; }

        public CourseRelatedDetails()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_related_list", Schema = "course")]
    public class CourseRelatedList
    {
        public long id { get; set; }
        public long courseRelatedId { get; set; }
        public long courseId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }
        public virtual CourseRelatedDetails CourseRelated { get; set; }

        public CourseRelatedList()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }


    [Table("course_language", Schema = "course")]
    public class CourseLanguage
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long languageId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Language language { get; set; }
        public virtual Courses Course { get; set; }


        public CourseLanguage()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_tags", Schema = "course")]
    public class CourseTag
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long tagId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Tags Tag { get; set; }
        public virtual Courses Course { get; set; }


        public CourseTag()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("learner_appraisal", Schema = "course")]
    public class LearnerAppraisal
    {
        public long id { get; set; }
        public long learnerId { get; set; }
        public long courseId { get; set; }
        public long appraisalId { get; set; }
        public string recommendation { get; set; }
        public int rating { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Appraisal Appraisal { get; set; }
        public LearnerAppraisal()
        {
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Attendance
    /// </summary>

    [Table("attendance", Schema = "course")]
    public class Attendance
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public long userId { get; set; }
        public long sessionId { get; set; }
        public long courseId { get; set; }
        public long userGroupId { get; set; }
        public int isPresent { get; set; }
        public int isLate { get; set; }
        public int isAbsent { get; set; }
        public int isExcused { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    /// <summary>
    /// Course Assessment
    /// </summary>

    [Table("course_assessment", Schema = "course")]
    public class CourseAssessment
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public string title { get; set; }
        public long assessmentTypeId { get; set; }
        public long userGroupId { get; set; }
        public int passingGrade { get; set; }

        public byte isImmediate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string duration { get; set; }
        public byte isAttempts { get; set; }
        public int attempts { get; set; }
        public byte basedType { get; set; }
        public byte isShuffle { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual AssessmentType AssessmentType { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual Courses Course { get; set; }
        public virtual IList<CourseAssessmentItem> CourseAssessmentItem { get; set; }
        public virtual IList<LearnerCourseAssessment> LearnerCourseAssessment { get; set; }

        public CourseAssessment()
        {
            updatedAt = DateTime.Now;
        }

    }

    [Table("course_assessment_items", Schema = "course")]
    public class CourseAssessmentItem
    {
        public long id { get; set; }
        public long courseAssessmentid { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public long assessmentItemTypeId { get; set; }
        public byte isShuffle { get; set; }
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public byte isTrue { get; set; }
        public byte isFalse { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual AssessmentItemType AssessmentItemType { get; set; }
        public virtual CourseAssessment CourseAssessment { get; set; }
        public virtual IList<CourseAssessmentItemChoices> CourseAssessmentItemChoices { get; set; }
        public virtual IList<LearnerCourseAssessment> LearnerCourseAssessment { get; set; }

        public CourseAssessmentItem()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_assessment_items_choices", Schema = "course")]
    public class CourseAssessmentItemChoices
    {
        public long id { get; set; }
        public long courseAssessmentItemId { get; set; }
        public string name { get; set; }
        public byte isCorrect { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public CourseAssessmentItemChoices()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Competencies
    /// </summary>

    [Table("course_competencies", Schema = "course")]
    public class CourseCompetencies
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long userGroupId { get; set; }
        public int lessonCompleted { get; set; }
        public int milestonesReached { get; set; }
        public int assessmentsSubmitted { get; set; }
        public int final { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual Courses Course { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual IList<CourseCompetenciesCertificate> CourseCompetenciesCertificate { get; set; }

        public CourseCompetencies()
        {
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Competencies
    /// </summary>

    [Table("course_competencies_certificate", Schema = "course")]
    public class CourseCompetenciesCertificate
    {
        public long id { get; set; }
        public long courseCompetenciesId { get; set; }
        public string attachment { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual CourseCompetencies CourseCompetencies { get; set; }
        public CourseCompetenciesCertificate()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Evaluation
    /// </summary>

    [Table("course_evaluation", Schema = "course")]
    public class CourseEvaluation
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public string title { get; set; }
        public long userGroupId { get; set; }
        public long evaluationTypeId { get; set; }
        public long evaluationActionId { get; set; }
        public byte isRequired { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual UserGroups UserGroup { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }
        public virtual EvaluationAction EvaluationAction { get; set; }
        public virtual IList<CourseEvaluationValues> CourseEvaluationValues { get; set; }

        public CourseEvaluation()
        {
            updatedAt = DateTime.Now;
        }
    }

    [Table("course_evaluation_values", Schema = "course")]
    public class CourseEvaluationValues
    {
        public long id { get; set; }
        public long courseEvaluationId { get; set; }
        public string name { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public CourseEvaluationValues()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Outcomes 
    /// </summary>

    [Table("course_outcome", Schema = "course")]
    public class CourseOutcome
    {
        public long id { get; set; }
        public string title { get; set; }
        public long courseId { get; set; }
        public long userGroupId { get; set; }
        public byte visibility { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual Courses Course { get; set; }

        public CourseOutcome()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Outline 
    /// </summary>

    [Table("course_outline", Schema = "course")]
    public class CourseOutline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string title { get; set; }
        public long courseId { get; set; }
        public long userGroupId { get; set; }
        public byte visibility { get; set; }
        public string featureImage { get; set; }
        public string interactiveVideo { get; set; }
        public int duration { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual IList<CourseOutlinePrerequisite> CourseOutlinePrerequisite { get; set; }
        public virtual IList<CourseOutlineMedia> CourseOutlineMedia { get; set; }
        public virtual IList<CourseOutlineMilestone> CourseOutlineMilestone { get; set; }
        public virtual IList<LearnerCourseOutline> LearnerCourseOutline { get; set; }


        public CourseOutline()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }

    [Table("course_outline_prerequisite", Schema = "course")]
    public class CourseOutlinePrerequisite
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long courseOutlineId { get; set; }
        public long preRequisiteId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public CoursePrerequisite Prerequisite { get; set; }
        public virtual Courses Course { get; set; }

        public CourseOutlinePrerequisite()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }

    [Table("course_outline_media", Schema = "course")]
    public class CourseOutlineMedia
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long courseOutlineId { get; set; }
        public string resourceFile { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }

        public CourseOutlineMedia()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }


    [Table("course_outline_milestone", Schema = "course")]
    public class CourseOutlineMilestone
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long courseOutlineId { get; set; }
        public string name { get; set; }
        public int lessonCompleted { get; set; }
        public string resourceFile { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }

        public CourseOutlineMilestone()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Course Session 
    /// </summary>

    [Table("sessions", Schema = "course")]
    public class CourseSession
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long sessionTypeId { get; set; }
        public string sessionLocation { get; set; }
        public long userGroupId { get; set; }
        public int capacity { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string duration { get; set; }
        public long courseInstructorId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual IList<LearnerSession> LearnerSession { get; set; }
        public virtual Courses Course { get; set; }
        public virtual SessionType SessionType { get; set; }
        public UserGroups UserGroup { get; set; }
        public virtual CourseInstructor CourseInstructor { get; set; }
    }


    [Table("learner", Schema = "course")]
    public class Learner
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long userId { get; set; }
        public byte enrollmentType { get; set; }
        public long statusId { get; set; }
        public long assessmentStatusId { get; set; }
        public byte isRecommendCourse { get; set; }
        public int instructorRating { get; set; }
        public int courseRating { get; set; }
        public string courseReview { get; set; }
        public int finalScore { get; set; }
        public int totalHoursTaken { get; set; }
        public byte isNotify { get; set; }
        public byte isApproved { get; set; }
        public string notificationDetails { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? appraisalDate { get; set; }
        public int overallRating { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Users User { get; set; }
        public virtual IList<LearnerSession> LearnerSession { get; set; }
        public virtual IList<LearnerCourseAssessment> LearnerCourseAssessment { get; set; }
        public virtual IList<LearnerAppraisal> LearnerAppraisal { get; set; }

        public virtual Status Status { get; set; }
        public virtual Status AssessmentStatus { get; set; }
        public virtual Courses Course { get; set; }


        public Learner()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }



    [Table("learner_course_assessment", Schema = "course")]
    public class LearnerCourseAssessment
    {
        public long id { get; set; }
        public long learnerId { get; set; }
        public long courseAssessmentId { get; set; }
        public long courseAssessmentItemId { get; set; }
        public long statusId { get; set; }
        public string answer { get; set; }
        public int points { get; set; }
        public int hoursTaken { get; set; }
        public DateTime dateTaken { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Learner Learner { get; set; }

        public virtual CourseAssessment CourseAssessment { get; set; }
        public virtual CourseAssessmentItem CourseAssessmentItem { get; set; }

        public LearnerCourseAssessment()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }



    [Table("learner_course_assessment_reminder", Schema = "course")]
    public class LearnerCourseAssessmentReminder
    {
        public long id { get; set; }
        public long learnerId { get; set; }
        public long courseId { get; set; }
        public string subject { get; set; }
        public byte isSent { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public LearnerCourseAssessmentReminder()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }


    [Table("learner_course_outline", Schema = "course")]
    public class LearnerCourseOutline
    {
        public long id { get; set; }
        public long courseOutlineId { get; set; }
        public long learnerId { get; set; }
        public long statusId { get; set; }
        public DateTime? courseStart { get; set; }
        public DateTime? courseEnd { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal hoursTaken { get; set; }
        public int score { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual CourseOutline CourseOutline { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual Status Status { get; set; }

        public LearnerCourseOutline()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }


    [Table("learner_session", Schema = "course")]
    public class LearnerSession
    {
        public long id { get; set; }
        public long sessionId { get; set; }
        public long courseId { get; set; }
        public long learnerId { get; set; }
        public long statusId { get; set; }
        public DateTime? dateScheduled { get; set; }
        public DateTime? courseStart { get; set; }
        public DateTime? courseEnd { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal hoursTaken { get; set; }
        public int score { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual Courses Course { get; set; }
        public virtual CourseSession Session { get; set; }

        public LearnerSession()
        {
            updatedAt = DateTime.Now;
        }

    }


    /// <summary>
    /// Course Instructors 
    /// </summary>

    [Table("course_instructor", Schema = "course")]
    public class CourseInstructor
    {
        public long id { get; set; }
        public long courseId { get; set; }
        public long userId { get; set; }
        public long userGroupId { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual Courses Course { get; set; }
        public virtual Users User { get; set; }
        public virtual UserGroups Usergroup { get; set; }
        public virtual IList<CourseSession> Session { get; set; }
        public CourseInstructor()
        {
            updatedAt = DateTime.Now;
        }
    }





    public class addCourseRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public long status { get; set; }
        public string featureImage { get; set; }
        public string featureVideo { get; set; }
        public byte isPublished { get; set; }
        public int durationTime { get; set; }
        public string durationType { get; set; }

        public int passingGrade { get; set; }
        public int capacity { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public Level LevelIds { get; set; }
    }



    public class PublishCourse
    {
        public long courseId { get; set; }
        public string publishDescription { get; set; }
        public long lmsProfile { get; set; }
        public byte isVisible { get; set; }
        public string courseUrl { get; set; }
        public byte notifyInstructor { get; set; }
    }

    public class VMEnrollment
    {
        public long id { get; set; }
        public long userGroupId { get; set; }
        public long sessionId { get; set; }
        public byte isAutoEnroll { get; set; }
        public byte isNotify { get; set; }
        public string notificationDetails { get; set; }
        public string appraisalOverallRating { get; set; }
        public int courseRating { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual IList<Learner> Learner { get; set; }
        public virtual IList<LearnerSession> LearnerSession { get; set; }

    }

}

