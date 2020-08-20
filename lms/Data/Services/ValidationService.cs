using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using lms.Data.Helpers;
using lms.Models;
using Microsoft.EntityFrameworkCore;

namespace lms.Data.Services
{
    public class ValidationService : IValidationService
    {

        private readonly lmsContext _context;

        public ValidationService(lmsContext context)
        {
            _context = context;
        }

        public Users validateEmailExist(string email)
        {
            return _context.Users.Where(x => x.email == email).FirstOrDefault();
        }

        public Users validateEmpIdExist(string empId)
        {
            return _context.Users.Where(x => x.empId == empId).FirstOrDefault();
        }

        public Users validateUsernameExist(string username)
        {
            return _context.Users.Where(x => x.username == username).FirstOrDefault();
        }

        public bool IsValidEmail(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public Object registrationValidation(Users request)
        {
            GenericResult _result = new GenericResult();
            var ifSucceeded = true;
            string message = "";

            if (validateEmailExist(request.email) != null)
            {
                ifSucceeded = false;
                message = "Email Address is already exists.";
            }
            else if (IsValidEmail(request.email) == false)
            {
                ifSucceeded = false;
                message = "Email Address is not valid.";
            }
            else if (validateEmpIdExist(request.empId) != null)
            {
                ifSucceeded = false;
                message = "Employee ID " + request.empId + " is already used.";
            }
            else if (validateUsernameExist(request.empId) != null)
            {
                ifSucceeded = false;
                message = "Username is already taken. Please enter another username.";
            }


            
            _result = new GenericResult
            {
                Response = ifSucceeded,
                Message = message
            };

            return _result;
        }

        public object ValidateRequest(string model, RequiredFields request)
        {
            var arr = new ArrayList();
            switch (model)
            {
                #region Course Session
                case "Course Session":
                    if (request.CourseSession.courseId == 0) 
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseSession.sessionTypeId == 0)
                        arr.Add(new RequestValidationModel { Name = "Session Type ID", Parameter = "sessionTypeId", Message = "Please select session type" });
                    if (request.CourseSession.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                    if (request.CourseSession.capacity == 0)
                        arr.Add(new RequestValidationModel { Name = "Capacity", Parameter = "capacity", Message = "Please enter capacity" });
                    if (request.CourseSession.startDate == null)
                        arr.Add(new RequestValidationModel { Name = "Start Date", Parameter = "startDate", Message = "Please enter session start date" });
                    if (request.CourseSession.endDate == null)
                        arr.Add(new RequestValidationModel { Name = "End Date", Parameter = "endDate", Message = "Please enter session end date" });
                    if (request.CourseSession.duration == null)
                        arr.Add(new RequestValidationModel { Name = "Duration", Parameter = "duration", Message = "Please enter session duration" });
                    if (request.CourseSession.courseInstructorId == 0)
                        arr.Add(new RequestValidationModel { Name = "Instructor ID", Parameter = "instructorId", Message = "Please assign instructor" });
                    if (request.CourseSession.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter session title" });
                 break;
                #endregion Course Session

                #region Courses
                case "Courses":
                    if (request.Courses.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter course title" });
                    if (request.Courses.durationTime == 0)
                        arr.Add(new RequestValidationModel { Name = "Time Duration", Parameter = "durationTime", Message = "Please select time duration" });
                    if (request.Courses.durationType == null)
                        arr.Add(new RequestValidationModel { Name = "Duration Type", Parameter = "durationType", Message = "Please select duration type" });
                    if (request.Courses.passingGrade == 0)
                        arr.Add(new RequestValidationModel { Name = "Passing Grade", Parameter = "passingGrade", Message = "Please select passing grade" });
                    if (request.Courses.capacity == 0)
                        arr.Add(new RequestValidationModel { Name = "Capacity", Parameter = "capacity", Message = "Please enter capacity" });
                break;
                #endregion Courses

                #region Course Outline
                case "Course Outline":
                    if (request.CourseOutline.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter course outline title" });
                    if (request.CourseOutline.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseOutline.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                    if (request.CourseOutline.duration == 0)
                        arr.Add(new RequestValidationModel { Name = "Duration", Parameter = "duration", Message = "Please enter duration" });
                break;
                #endregion Course Outline

                #region Course Outline Media
                case "Course Outline Media":
                    if (request.CourseOutlineMedia.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseOutlineMedia.courseOutlineId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Outline ID", Parameter = "courseOutlineId", Message = "Please enter course outline" });
                break;
                #endregion Course Outline Media

                #region Course Outline Prerequisite
                case "Course Outline Prerequisite":
                    if (request.CourseOutlineMedia.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseOutlineMedia.courseOutlineId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Outline ID", Parameter = "courseOutlineId", Message = "Please enter course outline" });
                break;
                #endregion Course Outline Prerequisite

                #region Course Outline Milestone
                case "Course Outline Milestone":
                    if (request.CourseOutlineMilestone.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseOutlineMilestone.courseOutlineId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Outline ID", Parameter = "courseOutlineId", Message = "Please enter course outline" });
                    if (request.CourseOutlineMilestone.name == null)
                        arr.Add(new RequestValidationModel { Name = "Course Milestone Name", Parameter = "name", Message = "Please enter course milestone name" });
                    if (request.CourseOutlineMilestone.lessonCompleted == 0)
                        arr.Add(new RequestValidationModel { Name = "No. of Lessons Completed", Parameter = "lessonCompleted", Message = "Please select no of lessons completed" });
                    break;
                #endregion Course Outline Milestone

                #region Course Outcome
                case "Course Outcome":
                    if (request.CourseOutcome.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseOutcome.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter course outcome title" });
                    if (request.CourseOutcome.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                break;
                #endregion Course Outcome

                #region Course Assessment
                case "Course Assessment":
                    if (request.CourseAssessment.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseAssessment.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter course assessment title" });
                    if (request.CourseAssessment.assessmentTypeId == 0)
                        arr.Add(new RequestValidationModel { Name = "Assessment Type ID", Parameter = "assessmentTypeId", Message = "Please select assessment type" });
                    if (request.CourseAssessment.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                    if (request.CourseAssessment.passingGrade == 0)
                        arr.Add(new RequestValidationModel { Name = "Passing Grade", Parameter = "passingGrade", Message = "Please enter passing grade" });

                    if (request.CourseAssessment.isImmediate == 0)
                    {
                        if (request.CourseAssessment.fromDate == null)
                            arr.Add(new RequestValidationModel { Name = "From Date", Parameter = "fromDate", Message = "Please enter start date" });
                        if (request.CourseAssessment.toDate == null)
                            arr.Add(new RequestValidationModel { Name = "To Date", Parameter = "toDate", Message = "Please enter end date" });
                    }

                    if (request.CourseAssessment.basedType != 0)
                    {
                        if (request.CourseAssessment.duration == null)
                            arr.Add(new RequestValidationModel { Name = "Duration", Parameter = "duration", Message = "Please enter duration" });
                    }

                    if (request.CourseAssessment.isAttempts == 1)
                    {
                        if (request.CourseAssessment.attempts == 0)
                            arr.Add(new RequestValidationModel { Name = "Attempts", Parameter = "attempts", Message = "Please enter number of attempts available" });
                    }
                break;
                #endregion Course Assessment

                #region Instructor
                case "Instructor":
                    if (request.Instructor.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.Instructor.userId == 0)
                        arr.Add(new RequestValidationModel { Name = "User ID", Parameter = "userId", Message = "Please select user" });
                    if (request.Instructor.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                break;
                #endregion Instructor

                #region Competencies
                case "Competencies":
                    if (request.Competencies.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.Competencies.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter competency title" });
                    if (request.Competencies.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                    if (request.Competencies.description == null)
                        arr.Add(new RequestValidationModel { Name = "Description", Parameter = "description", Message = "Please enter description" });
                    if (request.Competencies.lessonCompleted == 0)
                        arr.Add(new RequestValidationModel { Name = "Lesson Completed", Parameter = "lessonCompleted", Message = "Please enter lesson completed" });
                    if (request.Competencies.milestonesReached == 0)
                        arr.Add(new RequestValidationModel { Name = "Milestones Reached", Parameter = "milestonesReached", Message = "Please enter milestones reached" });
                    if (request.Competencies.assessmentsSubmitted == 0)
                        arr.Add(new RequestValidationModel { Name = "Assessment Submitted", Parameter = "assessmentsSubmitted", Message = "Please enter assessment submitted" });
                    if (request.Competencies.final == 0)
                        arr.Add(new RequestValidationModel { Name = "Final Grade", Parameter = "final", Message = "Please enter final grade" });
                break;
                #endregion Competencies

                #region Course Evaluation
                case "Course Evaluation":
                    if (request.CourseEvaluation.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.CourseEvaluation.title == null)
                        arr.Add(new RequestValidationModel { Name = "Title", Parameter = "title", Message = "Please enter course evaluation title" });
                    if (request.CourseEvaluation.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please select user group" });
                    if (request.CourseEvaluation.evaluationTypeId == 0)
                        arr.Add(new RequestValidationModel { Name = "Evaluation Type ID", Parameter = "evaluationTypeId", Message = "Please select evaluation type" });
                    if (request.CourseEvaluation.evaluationActionId == 0)
                        arr.Add(new RequestValidationModel { Name = "Evaluation Action ID", Parameter = "evaluationActionId", Message = "Please select evaluation action" });
                break;
                #endregion Course Evaluation

                #region User Group
                case "User Group":
                    if (request.UserGroups.name == null)
                        arr.Add(new RequestValidationModel { Name = "Name", Parameter = "name", Message = "Please enter name of user group" });
                break;
                #endregion User Group

                #region Groups
                case "Groups":
                    if (request.Groups.userId == 0)
                        arr.Add(new RequestValidationModel { Name = "User ID", Parameter = "userId", Message = "Please enter user" });
                    if (request.Groups.userGroupId == 0)
                        arr.Add(new RequestValidationModel { Name = "User Group ID", Parameter = "userGroupId", Message = "Please enter user group" });
                    break;
                #endregion Groups

                #region Status
                case "Status":
                    if (request.Status.name == null)
                        arr.Add(new RequestValidationModel { Name = "Status Name", Parameter = "name", Message = "Please enter status name" });
                    if (request.Status.category == null)
                        arr.Add(new RequestValidationModel { Name = "Status Category", Parameter = "category", Message = "Please enter category name" });
                    if (request.Status.color == null)
                        arr.Add(new RequestValidationModel { Name = "Color", Parameter = "color", Message = "Please assign status color" });
                    break;
                #endregion Status

                #region Tags
                case "Tags":
                    if (request.Tags.name == null)
                        arr.Add(new RequestValidationModel { Name = "Tag Name", Parameter = "name", Message = "Please enter tag name" });
                break;
                #endregion Tags

                #region Session Type
                case "Session Type":
                    if (request.SessionType.name == null)
                        arr.Add(new RequestValidationModel { Name = "Session Type Name", Parameter = "name", Message = "Please enter session type name" });
                    break;
                #endregion Session Type

                #region Location
                case "Location":
                    if (request.Location.name == null)
                        arr.Add(new RequestValidationModel { Name = "Location Name", Parameter = "name", Message = "Please enter location name" });
                    if (request.Location.code == null)
                        arr.Add(new RequestValidationModel { Name = "Location Code", Parameter = "code", Message = "Please enter location code" });
                    break;
                #endregion Location

                #region Department
                case "Department":
                    if (request.Department.name == null)
                        arr.Add(new RequestValidationModel { Name = "Department Name", Parameter = "name", Message = "Please enter department name" });
                    if (request.Department.code == null)
                        arr.Add(new RequestValidationModel { Name = "Department Code", Parameter = "code", Message = "Please enter department code" });
                    break;
                #endregion Department

                #region Appraisal
                case "Appraisal":
                    if (request.Appraisal.name == null)
                        arr.Add(new RequestValidationModel { Name = "Appraisal Name", Parameter = "name", Message = "Please enter appraisal name" });
                    if (request.Appraisal.courseTypeId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Type ID", Parameter = "courseTypeId", Message = "Please select course type" });
                    break;
                #endregion Appraisal

                #region Category
                case "Category":
                    if (request.Category.name == null)
                        arr.Add(new RequestValidationModel { Name = "Category Name", Parameter = "name", Message = "Please enter category name" });
                break;
                #endregion Category

                #region Learner Course Outline
                case "Learner Course Outline":
                    if (request.LearnerCourseOutline.courseOutlineId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Outline ID", Parameter = "courseOutlineId", Message = "Please select course outline" });
                    if (request.LearnerCourseOutline.learnerId == 0)
                        arr.Add(new RequestValidationModel { Name = "Learner ID", Parameter = "learnerId", Message = "Please enter learner" });
                break;
                #endregion Learner Course Outline

                #region Learner Course Assessment
                case "Learner Course Assessment":
                    if (request.LearnerCourseAssessment.learnerId == 0)
                        arr.Add(new RequestValidationModel { Name = "Learner ID", Parameter = "learnerId", Message = "Please enter learner" });
                    if (request.LearnerCourseAssessment.courseAssessmentId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Assessment ID", Parameter = "courseAssessmentId", Message = "Please select course assessment" });
                    if (request.LearnerCourseAssessment.courseAssessmentItemId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course Assessment Item ID", Parameter = "courseAssessmentItemId", Message = "Please select course assessment item" });
                break;
                #endregion Learner Course Assessment

                #region Learner Appraisal
                case "Learner Appraisal":
                    if (request.LearnerAppraisalFilter.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    //if (request.LearnerAppraisal.learnerId == 0)
                    //    arr.Add(new RequestValidationModel { Name = "Learner ID", Parameter = "learnerId", Message = "Please enter learner" });
                    //if (request.LearnerAppraisal.appraisalId == 0)
                    //    arr.Add(new RequestValidationModel { Name = "Appraisal ID", Parameter = "appraisalId", Message = "Please select appraisal" });
                    break;
                #endregion Learner Appraisal

                #region Learner Course Assessment Reminder
                case "Learner Course Assessment Reminder":
                    if (request.LearnerCourseAssessmentReminder.courseId == 0)
                        arr.Add(new RequestValidationModel { Name = "Course ID", Parameter = "courseId", Message = "Please select course" });
                    if (request.LearnerCourseAssessmentReminder.learnerId == 0)
                        arr.Add(new RequestValidationModel { Name = "Learner ID", Parameter = "learnerId", Message = "Please enter learner" });
                    if (request.LearnerCourseAssessmentReminder.subject == null)
                        arr.Add(new RequestValidationModel { Name = "Subject", Parameter = "subject", Message = "Please enter subject" });
                    if (request.LearnerCourseAssessmentReminder.description == null)
                        arr.Add(new RequestValidationModel { Name = "Description", Parameter = "description", Message = "Please enter Description" });
                break;
                #endregion Learner Course Assessment Reminder

                #region Language
                case "Language":
                    if (request.Language.name == null)
                        arr.Add(new RequestValidationModel { Name = "Language Name", Parameter = "name", Message = "Please enter language name" });
                    break;
                #endregion Language

                #region Level
                case "Level":
                    if (request.Level.name == null)
                        arr.Add(new RequestValidationModel { Name = "Course Level Name", Parameter = "name", Message = "Please enter course level name" });
                    break;
                #endregion Level

                #region Types
                case "Types":
                    if (request.Types.name == null)
                        arr.Add(new RequestValidationModel { Name = "Course Type Name", Parameter = "name", Message = "Please enter course type name" });
                    break;
                #endregion Types

                default:
                break;
            }
            return arr;

        }
    }
}
