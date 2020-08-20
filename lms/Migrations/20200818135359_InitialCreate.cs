using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace lms.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "picklist");

            migrationBuilder.EnsureSchema(
                name: "course");

            migrationBuilder.EnsureSchema(
                name: "settings");

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    status = table.Column<long>(nullable: false),
                    featureImage = table.Column<string>(nullable: true),
                    featureVideo = table.Column<string>(nullable: true),
                    isPublished = table.Column<byte>(nullable: false),
                    requestForPublish = table.Column<byte>(nullable: false),
                    isVisible = table.Column<byte>(nullable: false),
                    durationTime = table.Column<int>(nullable: false),
                    durationType = table.Column<string>(nullable: true),
                    passingGrade = table.Column<int>(nullable: false),
                    capacity = table.Column<int>(nullable: false),
                    notifyInstructor = table.Column<byte>(nullable: false),
                    lmsProfile = table.Column<long>(nullable: false),
                    publishDescription = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "learner_course_assessment_reminder",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learnerId = table.Column<long>(nullable: false),
                    courseId = table.Column<long>(nullable: false),
                    subject = table.Column<string>(nullable: true),
                    isSent = table.Column<byte>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner_course_assessment_reminder", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assessment_item_type",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_item_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assessment_type",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_category",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_level",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_level", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_prerequisite",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_prerequisite", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_type",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "department",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_action",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_action", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_type",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "session_type",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                schema: "settings",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    color = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_groups",
                schema: "settings",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "settings",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empId = table.Column<string>(nullable: true),
                    firstName = table.Column<string>(nullable: true),
                    middleInitial = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    salt = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    projId = table.Column<long>(nullable: false),
                    deptId = table.Column<long>(nullable: false),
                    positionId = table.Column<long>(nullable: false),
                    internationalStatusId = table.Column<long>(nullable: false),
                    obsId = table.Column<long>(nullable: false),
                    branchId = table.Column<long>(nullable: false),
                    isActive = table.Column<byte>(nullable: false),
                    birthday = table.Column<DateTime>(nullable: true),
                    gender = table.Column<byte>(nullable: false),
                    isInstructor = table.Column<byte>(nullable: false),
                    isAdministrator = table.Column<byte>(nullable: false),
                    isLearner = table.Column<byte>(nullable: false),
                    canCreate = table.Column<byte>(nullable: false),
                    canModify = table.Column<byte>(nullable: false),
                    canRemove = table.Column<byte>(nullable: false),
                    dateApproved = table.Column<DateTime>(nullable: true),
                    hireDate = table.Column<DateTime>(nullable: true),
                    lastWorkingDate = table.Column<DateTime>(nullable: true),
                    token = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_related",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    isPrerequisite = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_related", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_related_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_category",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    categoryId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_category", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_category_course_category_categoryId",
                        column: x => x.categoryId,
                        principalSchema: "picklist",
                        principalTable: "course_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_category_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_level",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    levelId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_level", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_level_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_level_course_level_levelId",
                        column: x => x.levelId,
                        principalSchema: "picklist",
                        principalTable: "course_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_type",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    courseTypeId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_type_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_type_course_type_courseTypeId",
                        column: x => x.courseTypeId,
                        principalSchema: "picklist",
                        principalTable: "course_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appraisal",
                schema: "picklist",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseTypeId = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    isEditable = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appraisal", x => x.id);
                    table.ForeignKey(
                        name: "FK_appraisal_course_type_courseTypeId",
                        column: x => x.courseTypeId,
                        principalSchema: "picklist",
                        principalTable: "course_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_language",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    languageId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_language", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_language_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_language_language_languageId",
                        column: x => x.languageId,
                        principalSchema: "picklist",
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_tags",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    tagId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_tags_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_tags_tags_tagId",
                        column: x => x.tagId,
                        principalSchema: "picklist",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_assessment",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    assessmentTypeId = table.Column<long>(nullable: false),
                    userGroupId = table.Column<long>(nullable: false),
                    passingGrade = table.Column<int>(nullable: false),
                    isImmediate = table.Column<byte>(nullable: false),
                    fromDate = table.Column<DateTime>(nullable: true),
                    toDate = table.Column<DateTime>(nullable: true),
                    duration = table.Column<string>(nullable: true),
                    isAttempts = table.Column<byte>(nullable: false),
                    attempts = table.Column<int>(nullable: false),
                    basedType = table.Column<byte>(nullable: false),
                    isShuffle = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_assessment", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_assessment_assessment_type_assessmentTypeId",
                        column: x => x.assessmentTypeId,
                        principalSchema: "picklist",
                        principalTable: "assessment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_assessment_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_assessment_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_competencies",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    userGroupId = table.Column<long>(nullable: false),
                    lessonCompleted = table.Column<int>(nullable: false),
                    milestonesReached = table.Column<int>(nullable: false),
                    assessmentsSubmitted = table.Column<int>(nullable: false),
                    final = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_competencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_competencies_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_competencies_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_evaluation",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    userGroupId = table.Column<long>(nullable: false),
                    evaluationTypeId = table.Column<long>(nullable: false),
                    evaluationActionId = table.Column<long>(nullable: false),
                    isRequired = table.Column<byte>(nullable: false),
                    minValue = table.Column<int>(nullable: false),
                    maxValue = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_evaluation", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_evaluation_evaluation_action_evaluationActionId",
                        column: x => x.evaluationActionId,
                        principalSchema: "picklist",
                        principalTable: "evaluation_action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_evaluation_evaluation_type_evaluationTypeId",
                        column: x => x.evaluationTypeId,
                        principalSchema: "picklist",
                        principalTable: "evaluation_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_evaluation_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_outcome",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true),
                    courseId = table.Column<long>(nullable: false),
                    userGroupId = table.Column<long>(nullable: false),
                    visibility = table.Column<byte>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_outcome", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_outcome_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outcome_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_outline",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true),
                    courseId = table.Column<long>(nullable: false),
                    userGroupId = table.Column<long>(nullable: false),
                    visibility = table.Column<byte>(nullable: false),
                    featureImage = table.Column<string>(nullable: true),
                    interactiveVideo = table.Column<string>(nullable: true),
                    duration = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_outline", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_outline_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outline_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_instructor",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    userId = table.Column<long>(nullable: false),
                    userGroupId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_instructor", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_instructor_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_instructor_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_instructor_users_userId",
                        column: x => x.userId,
                        principalSchema: "settings",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learner",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    userId = table.Column<long>(nullable: false),
                    enrollmentType = table.Column<byte>(nullable: false),
                    statusId = table.Column<long>(nullable: false),
                    assessmentStatusId = table.Column<long>(nullable: false),
                    isRecommendCourse = table.Column<byte>(nullable: false),
                    instructorRating = table.Column<int>(nullable: false),
                    courseRating = table.Column<int>(nullable: false),
                    courseReview = table.Column<string>(nullable: true),
                    finalScore = table.Column<int>(nullable: false),
                    totalHoursTaken = table.Column<int>(nullable: false),
                    isNotify = table.Column<byte>(nullable: false),
                    isApproved = table.Column<byte>(nullable: false),
                    notificationDetails = table.Column<string>(nullable: true),
                    startDate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    appraisalDate = table.Column<DateTime>(nullable: true),
                    overallRating = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner", x => x.id);
                    table.ForeignKey(
                        name: "FK_learner_status_assessmentStatusId",
                        column: x => x.assessmentStatusId,
                        principalSchema: "settings",
                        principalTable: "status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_status_statusId",
                        column: x => x.statusId,
                        principalSchema: "settings",
                        principalTable: "status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_users_userId",
                        column: x => x.userId,
                        principalSchema: "settings",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "settings",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<long>(nullable: false),
                    userGroupId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_groups_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_groups_users_userId",
                        column: x => x.userId,
                        principalSchema: "settings",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_related_list",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseRelatedId = table.Column<long>(nullable: false),
                    courseId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_related_list", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_related_list_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_related_list_course_related_courseRelatedId",
                        column: x => x.courseRelatedId,
                        principalSchema: "course",
                        principalTable: "course_related",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_assessment_items",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseAssessmentid = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    duration = table.Column<int>(nullable: false),
                    assessmentItemTypeId = table.Column<long>(nullable: false),
                    isShuffle = table.Column<byte>(nullable: false),
                    minLength = table.Column<int>(nullable: false),
                    maxLength = table.Column<int>(nullable: false),
                    isTrue = table.Column<byte>(nullable: false),
                    isFalse = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_assessment_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_assessment_items_assessment_item_type_assessmentItemTypeId",
                        column: x => x.assessmentItemTypeId,
                        principalSchema: "picklist",
                        principalTable: "assessment_item_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_assessment_items_course_assessment_courseAssessmentid",
                        column: x => x.courseAssessmentid,
                        principalSchema: "course",
                        principalTable: "course_assessment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_competencies_certificate",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseCompetenciesId = table.Column<long>(nullable: false),
                    attachment = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_competencies_certificate", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_competencies_certificate_course_competencies_courseCompetenciesId",
                        column: x => x.courseCompetenciesId,
                        principalSchema: "course",
                        principalTable: "course_competencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_evaluation_values",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseEvaluationId = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_evaluation_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_evaluation_values_course_evaluation_courseEvaluationId",
                        column: x => x.courseEvaluationId,
                        principalSchema: "course",
                        principalTable: "course_evaluation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_outline_media",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    courseOutlineId = table.Column<long>(nullable: false),
                    resourceFile = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_outline_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_outline_media_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outline_media_course_outline_courseOutlineId",
                        column: x => x.courseOutlineId,
                        principalSchema: "course",
                        principalTable: "course_outline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_outline_milestone",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    courseOutlineId = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    lessonCompleted = table.Column<int>(nullable: false),
                    resourceFile = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_outline_milestone", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_outline_milestone_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outline_milestone_course_outline_courseOutlineId",
                        column: x => x.courseOutlineId,
                        principalSchema: "course",
                        principalTable: "course_outline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_outline_prerequisite",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    courseOutlineId = table.Column<long>(nullable: false),
                    preRequisiteId = table.Column<long>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_outline_prerequisite", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_outline_prerequisite_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outline_prerequisite_course_outline_courseOutlineId",
                        column: x => x.courseOutlineId,
                        principalSchema: "course",
                        principalTable: "course_outline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_outline_prerequisite_course_prerequisite_preRequisiteId",
                        column: x => x.preRequisiteId,
                        principalSchema: "picklist",
                        principalTable: "course_prerequisite",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<long>(nullable: false),
                    sessionTypeId = table.Column<long>(nullable: false),
                    sessionLocation = table.Column<string>(nullable: true),
                    userGroupId = table.Column<long>(nullable: false),
                    capacity = table.Column<int>(nullable: false),
                    startDate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    duration = table.Column<string>(nullable: true),
                    courseInstructorId = table.Column<long>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessions_course_instructor_courseInstructorId",
                        column: x => x.courseInstructorId,
                        principalSchema: "course",
                        principalTable: "course_instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessions_session_type_sessionTypeId",
                        column: x => x.sessionTypeId,
                        principalSchema: "picklist",
                        principalTable: "session_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessions_user_groups_userGroupId",
                        column: x => x.userGroupId,
                        principalSchema: "settings",
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learner_appraisal",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learnerId = table.Column<long>(nullable: false),
                    courseId = table.Column<long>(nullable: false),
                    appraisalId = table.Column<long>(nullable: false),
                    recommendation = table.Column<string>(nullable: true),
                    rating = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner_appraisal", x => x.id);
                    table.ForeignKey(
                        name: "FK_learner_appraisal_appraisal_appraisalId",
                        column: x => x.appraisalId,
                        principalSchema: "picklist",
                        principalTable: "appraisal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_appraisal_learner_learnerId",
                        column: x => x.learnerId,
                        principalSchema: "course",
                        principalTable: "learner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learner_course_outline",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseOutlineId = table.Column<long>(nullable: false),
                    learnerId = table.Column<long>(nullable: false),
                    statusId = table.Column<long>(nullable: false),
                    courseStart = table.Column<DateTime>(nullable: true),
                    courseEnd = table.Column<DateTime>(nullable: true),
                    hoursTaken = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    score = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner_course_outline", x => x.id);
                    table.ForeignKey(
                        name: "FK_learner_course_outline_course_outline_courseOutlineId",
                        column: x => x.courseOutlineId,
                        principalSchema: "course",
                        principalTable: "course_outline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_course_outline_learner_learnerId",
                        column: x => x.learnerId,
                        principalSchema: "course",
                        principalTable: "learner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_course_outline_status_statusId",
                        column: x => x.statusId,
                        principalSchema: "settings",
                        principalTable: "status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_assessment_items_choices",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseAssessmentItemId = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    isCorrect = table.Column<byte>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_assessment_items_choices", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_assessment_items_choices_course_assessment_items_courseAssessmentItemId",
                        column: x => x.courseAssessmentItemId,
                        principalSchema: "course",
                        principalTable: "course_assessment_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learner_course_assessment",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    learnerId = table.Column<long>(nullable: false),
                    courseAssessmentId = table.Column<long>(nullable: false),
                    courseAssessmentItemId = table.Column<long>(nullable: false),
                    statusId = table.Column<long>(nullable: false),
                    answer = table.Column<string>(nullable: true),
                    points = table.Column<int>(nullable: false),
                    hoursTaken = table.Column<int>(nullable: false),
                    dateTaken = table.Column<DateTime>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner_course_assessment", x => x.id);
                    table.ForeignKey(
                        name: "FK_learner_course_assessment_course_assessment_courseAssessmentId",
                        column: x => x.courseAssessmentId,
                        principalSchema: "course",
                        principalTable: "course_assessment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_course_assessment_course_assessment_items_courseAssessmentItemId",
                        column: x => x.courseAssessmentItemId,
                        principalSchema: "course",
                        principalTable: "course_assessment_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_course_assessment_learner_learnerId",
                        column: x => x.learnerId,
                        principalSchema: "course",
                        principalTable: "learner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learner_session",
                schema: "course",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sessionId = table.Column<long>(nullable: false),
                    courseId = table.Column<long>(nullable: false),
                    learnerId = table.Column<long>(nullable: false),
                    statusId = table.Column<long>(nullable: false),
                    dateScheduled = table.Column<DateTime>(nullable: true),
                    courseStart = table.Column<DateTime>(nullable: true),
                    courseEnd = table.Column<DateTime>(nullable: true),
                    hoursTaken = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    score = table.Column<int>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learner_session", x => x.id);
                    table.ForeignKey(
                        name: "FK_learner_session_courses_courseId",
                        column: x => x.courseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_session_learner_learnerId",
                        column: x => x.learnerId,
                        principalSchema: "course",
                        principalTable: "learner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_learner_session_sessions_sessionId",
                        column: x => x.sessionId,
                        principalSchema: "course",
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_assessmentTypeId",
                schema: "course",
                table: "course_assessment",
                column: "assessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_courseId",
                schema: "course",
                table: "course_assessment",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_userGroupId",
                schema: "course",
                table: "course_assessment",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_items_assessmentItemTypeId",
                schema: "course",
                table: "course_assessment_items",
                column: "assessmentItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_items_courseAssessmentid",
                schema: "course",
                table: "course_assessment_items",
                column: "courseAssessmentid");

            migrationBuilder.CreateIndex(
                name: "IX_course_assessment_items_choices_courseAssessmentItemId",
                schema: "course",
                table: "course_assessment_items_choices",
                column: "courseAssessmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_course_category_categoryId",
                schema: "course",
                table: "course_category",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_course_category_courseId",
                schema: "course",
                table: "course_category",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_competencies_courseId",
                schema: "course",
                table: "course_competencies",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_competencies_userGroupId",
                schema: "course",
                table: "course_competencies",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_competencies_certificate_courseCompetenciesId",
                schema: "course",
                table: "course_competencies_certificate",
                column: "courseCompetenciesId");

            migrationBuilder.CreateIndex(
                name: "IX_course_evaluation_evaluationActionId",
                schema: "course",
                table: "course_evaluation",
                column: "evaluationActionId");

            migrationBuilder.CreateIndex(
                name: "IX_course_evaluation_evaluationTypeId",
                schema: "course",
                table: "course_evaluation",
                column: "evaluationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_course_evaluation_userGroupId",
                schema: "course",
                table: "course_evaluation",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_evaluation_values_courseEvaluationId",
                schema: "course",
                table: "course_evaluation_values",
                column: "courseEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructor_courseId",
                schema: "course",
                table: "course_instructor",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructor_userGroupId",
                schema: "course",
                table: "course_instructor",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructor_userId",
                schema: "course",
                table: "course_instructor",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_course_language_courseId",
                schema: "course",
                table: "course_language",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_language_languageId",
                schema: "course",
                table: "course_language",
                column: "languageId");

            migrationBuilder.CreateIndex(
                name: "IX_course_level_courseId",
                schema: "course",
                table: "course_level",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_level_levelId",
                schema: "course",
                table: "course_level",
                column: "levelId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outcome_courseId",
                schema: "course",
                table: "course_outcome",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outcome_userGroupId",
                schema: "course",
                table: "course_outcome",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_courseId",
                schema: "course",
                table: "course_outline",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_userGroupId",
                schema: "course",
                table: "course_outline",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_media_courseId",
                schema: "course",
                table: "course_outline_media",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_media_courseOutlineId",
                schema: "course",
                table: "course_outline_media",
                column: "courseOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_milestone_courseId",
                schema: "course",
                table: "course_outline_milestone",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_milestone_courseOutlineId",
                schema: "course",
                table: "course_outline_milestone",
                column: "courseOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_prerequisite_courseId",
                schema: "course",
                table: "course_outline_prerequisite",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_prerequisite_courseOutlineId",
                schema: "course",
                table: "course_outline_prerequisite",
                column: "courseOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_course_outline_prerequisite_preRequisiteId",
                schema: "course",
                table: "course_outline_prerequisite",
                column: "preRequisiteId");

            migrationBuilder.CreateIndex(
                name: "IX_course_related_courseId",
                schema: "course",
                table: "course_related",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_related_list_courseId",
                schema: "course",
                table: "course_related_list",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_related_list_courseRelatedId",
                schema: "course",
                table: "course_related_list",
                column: "courseRelatedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_tags_courseId",
                schema: "course",
                table: "course_tags",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_tags_tagId",
                schema: "course",
                table: "course_tags",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_course_type_courseId",
                schema: "course",
                table: "course_type",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_type_courseTypeId",
                schema: "course",
                table: "course_type",
                column: "courseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_assessmentStatusId",
                schema: "course",
                table: "learner",
                column: "assessmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_courseId",
                schema: "course",
                table: "learner",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_statusId",
                schema: "course",
                table: "learner",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_userId",
                schema: "course",
                table: "learner",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_appraisal_appraisalId",
                schema: "course",
                table: "learner_appraisal",
                column: "appraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_appraisal_learnerId",
                schema: "course",
                table: "learner_appraisal",
                column: "learnerId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_assessment_courseAssessmentId",
                schema: "course",
                table: "learner_course_assessment",
                column: "courseAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_assessment_courseAssessmentItemId",
                schema: "course",
                table: "learner_course_assessment",
                column: "courseAssessmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_assessment_learnerId",
                schema: "course",
                table: "learner_course_assessment",
                column: "learnerId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_outline_courseOutlineId",
                schema: "course",
                table: "learner_course_outline",
                column: "courseOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_outline_learnerId",
                schema: "course",
                table: "learner_course_outline",
                column: "learnerId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_course_outline_statusId",
                schema: "course",
                table: "learner_course_outline",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_session_courseId",
                schema: "course",
                table: "learner_session",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_session_learnerId",
                schema: "course",
                table: "learner_session",
                column: "learnerId");

            migrationBuilder.CreateIndex(
                name: "IX_learner_session_sessionId",
                schema: "course",
                table: "learner_session",
                column: "sessionId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_courseId",
                schema: "course",
                table: "sessions",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_courseInstructorId",
                schema: "course",
                table: "sessions",
                column: "courseInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_sessionTypeId",
                schema: "course",
                table: "sessions",
                column: "sessionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_userGroupId",
                schema: "course",
                table: "sessions",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisal_courseTypeId",
                schema: "picklist",
                table: "appraisal",
                column: "courseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_userGroupId",
                schema: "settings",
                table: "groups",
                column: "userGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_userId",
                schema: "settings",
                table: "groups",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_assessment_items_choices",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_category",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_competencies_certificate",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_evaluation_values",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_language",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_level",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_outcome",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_outline_media",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_outline_milestone",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_outline_prerequisite",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_related_list",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_tags",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_type",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner_appraisal",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner_course_assessment",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner_course_assessment_reminder",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner_course_outline",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner_session",
                schema: "course");

            migrationBuilder.DropTable(
                name: "department",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "location",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "settings");

            migrationBuilder.DropTable(
                name: "course_category",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_competencies",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_evaluation",
                schema: "course");

            migrationBuilder.DropTable(
                name: "language",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_level",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_prerequisite",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_related",
                schema: "course");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "appraisal",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_assessment_items",
                schema: "course");

            migrationBuilder.DropTable(
                name: "course_outline",
                schema: "course");

            migrationBuilder.DropTable(
                name: "learner",
                schema: "course");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "course");

            migrationBuilder.DropTable(
                name: "evaluation_action",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "evaluation_type",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_type",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "assessment_item_type",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "course_assessment",
                schema: "course");

            migrationBuilder.DropTable(
                name: "status",
                schema: "settings");

            migrationBuilder.DropTable(
                name: "course_instructor",
                schema: "course");

            migrationBuilder.DropTable(
                name: "session_type",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "assessment_type",
                schema: "picklist");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "course");

            migrationBuilder.DropTable(
                name: "user_groups",
                schema: "settings");

            migrationBuilder.DropTable(
                name: "users",
                schema: "settings");
        }
    }
}
