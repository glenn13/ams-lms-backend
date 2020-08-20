using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories.Abstract
{
    #region Interface Declaration
    public interface IAppraisalRepository : IEntityBaseRepository<Appraisal> { }
    //public interface ICategoryRepository : IEntityBaseRepository<Category> { }
    //public interface IClassRepository : IEntityBaseRepository<CourseInstructor> { }
    //public interface ICompetenciesRepository : IEntityBaseRepository<CourseCompetencies> { }
    //public interface ICourseAssessmentRepository : IEntityBaseRepository<CourseAssessment> { }
    //public interface ICourseEvaluationRepository : IEntityBaseRepository<CourseEvaluation> { }
    //public interface ICourseOutcomeRepository : IEntityBaseRepository<CourseOutcome> { }
    //public interface ICourseOutlineRepository : IEntityBaseRepository<CourseOutline> { }
    //public interface ICourseRepository : IEntityBaseRepository<Courses> { }
    //public interface ICourseSessionRepository : IEntityBaseRepository<CourseSession> { }
    //public interface IDepartmentRepository : IEntityBaseRepository<Department> { }
    //public interface IEnrollmentRepository : IEntityBaseRepository<Learner> { }
    //public interface IGroupRepository : IEntityBaseRepository<Groups> { }
    //public interface IInstructorRepository : IEntityBaseRepository<CourseInstructor> { }
    //public interface ILanguageRepository : IEntityBaseRepository<Language> { }
    //public interface ILearnerRepository : IEntityBaseRepository<Learner> { }
    //public interface ILevelRepository : IEntityBaseRepository<Level> { }
    //public interface ILocationRepository : IEntityBaseRepository<Location> { }
    //public interface ISessionTypeRepository : IEntityBaseRepository<SessionType> { }
    //public interface IStatusRepository : IEntityBaseRepository<Status> { }
    //public interface ITagsRepository : IEntityBaseRepository<Tags> { }
    //public interface ITypesRepository : IEntityBaseRepository<Types> { }
    //public interface IUserGroupRepository : IEntityBaseRepository<UserGroups> { }
    //public interface IUserRepository : IEntityBaseRepository<Users> { }
    #endregion


    public class AppraisalRepository : EntityBaseRepository<Appraisal>, IAppraisalRepository
    {
        public AppraisalRepository(lmsContext _db) : base(_db) { }
    }
}
