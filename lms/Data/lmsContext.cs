using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lms.Models;

namespace lms.Data
{
    public class lmsContext : DbContext
    {
        public lmsContext(DbContextOptions<lmsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int baseId = 100000;
            int empId = baseId + 1;
            var data = new byte[0x10];
            var crytoServiceProvider = System.Security.Cryptography.RandomNumberGenerator.Create();
            crytoServiceProvider.GetBytes(data);


            modelBuilder.Entity<Users>()
            .HasData(
                new Users
                {
                    id = 1,
                    empId = Convert.ToString(empId),
                    password = "123456",
                    salt = Convert.ToBase64String(data),
                    firstName = "Admin",
                    lastName = "Admin", 
                    username = "admin", 
                    email = "admin@ams.global", 
                    isAdministrator = 1, 
                    canCreate = 1,
                    canModify = 1, 
                    canRemove = 1,
                    hireDate = DateTime.Now,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                }
            );

        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseLevel> CourseLevel { get; set; }
        public DbSet<CourseCategory> CourseCategory { get; set; }
        public DbSet<CourseType> CourseType { get; set; }
        public DbSet<CourseRelatedDetails> CourseRelated { get; set; }
        public DbSet<CourseRelatedList> CourseRelatedList { get; set; }
        public DbSet<CourseLanguage> CourseLanguage { get; set; }
        public DbSet<CourseTag> CourseTag { get; set; }
        public DbSet<CourseOutline> CourseOutline { get; set; }
        public DbSet<CourseOutlinePrerequisite> CourseOutlinePrerequisite { get; set; }
        public DbSet<CourseOutlineMedia> CourseOutlineMedia { get; set; }
        public DbSet<CourseOutlineMilestone> CourseOutlineMilestone { get; set; }
        public DbSet<CourseOutcome> CourseOutcome { get; set; }
        public DbSet<CourseAssessment> CourseAssessment { get; set; }
        public DbSet<CourseAssessmentItem> CourseAssessmentItem { get; set; }
        public DbSet<CourseAssessmentItemChoices> CourseAssessmentItemChoices { get; set; }
        public DbSet<CourseInstructor> CourseInstructor { get; set; }
        public DbSet<CourseCompetencies> CourseCompetencies { get; set; }
        public DbSet<CourseCompetenciesCertificate> CourseCompetenciesCertificate { get; set; }
        public DbSet<CourseEvaluation> CourseEvaluation { get; set; }
        public DbSet<CourseEvaluationValues> CourseEvaluationValues { get; set; }
        public DbSet<CourseSession> CourseSession { get; set; }
        public DbSet<SessionType> SessionType { get; set; }
        public DbSet<Learner> Learner { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<LearnerCourseOutline> LearnerCourseOutline { get; set; }
        public DbSet<LearnerCourseAssessment> LearnerCourseAssessment { get; set; }
        public DbSet<LearnerSession> LearnerSession { get; set; }
        public DbSet<LearnerAppraisal> LearnerAppraisal { get; set; }
        public DbSet<Appraisal> Appraisal { get; set; }
        public DbSet<LearnerCourseAssessmentReminder> LearnerCourseAssessmentReminder { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Types> Types { get; set; }
    }
}
