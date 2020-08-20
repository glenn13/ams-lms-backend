using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lms.Models;

namespace lms.Data.Repositories
{
    public interface ILearnerRepository
    {
        int EnrollByInstructor(long courseId, long sessionId, VMEnrollment request);
        bool UpdateLeanerEnrollment(long learnerId, long sessionId, Learner request);
        bool AddCourseOutline(LearnerCourseOutline request);
        bool AddCourseAssessment(LearnerCourseAssessment request);
        bool AssessLearner(long learnerId, Learner request);
        IEnumerable<Learner> GetLearnerAssessment(long userId);
        IEnumerable<Learner> GetLearnerCompetencies(long userId);
        IEnumerable<Learner> GetLearnerAppraisal(long userId);
        object AddLearnerAppraisal(long learnerId, Learner request);
        bool Delete(long learnerId);
        bool DeleteLearnerSession(long learnerSessionId);
        bool ApproveLearner(long learnerId);
        bool AddCourseReview(long learnerId, Learner request);
        object LearnerCourse(long userId);
    }
}
