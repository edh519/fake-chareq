using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccessLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestStatuses",
                columns: table => new
                {
                    ChangeRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestStatuses", x => x.ChangeRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "DecisionTypes",
                columns: table => new
                {
                    DecisionTypeId = table.Column<int>(type: "int", nullable: false),
                    DecisionTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionTypes", x => x.DecisionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmailTypes",
                columns: table => new
                {
                    EmailTypeId = table.Column<int>(type: "int", nullable: false),
                    EmailTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTypes", x => x.EmailTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ImpactTypes",
                columns: table => new
                {
                    ImpactTypeId = table.Column<int>(type: "int", nullable: false),
                    ImpactTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpactTypes", x => x.ImpactTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    NotificationTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.NotificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDeviationReasons",
                columns: table => new
                {
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "varchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDeviationReasons", x => x.ProcessDeviationReasonId);
                });

            migrationBuilder.CreateTable(
                name: "RationaleTypes",
                columns: table => new
                {
                    RationaleTypeId = table.Column<int>(type: "int", nullable: false),
                    RationaleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RationaleTypes", x => x.RationaleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReasonTypes",
                columns: table => new
                {
                    RejectionReasonTypeId = table.Column<int>(type: "int", nullable: false),
                    RejectionReasonTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CausesArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReasonTypes", x => x.RejectionReasonTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SignatureUploads",
                columns: table => new
                {
                    SignatureUploadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileUploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignatureUploads", x => x.SignatureUploadId);
                });

            migrationBuilder.CreateTable(
                name: "Trials",
                columns: table => new
                {
                    TrialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trials", x => x.TrialId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusChangeRequestStatusId = table.Column<int>(type: "int", nullable: true),
                    HasActiveRejection = table.Column<bool>(type: "bit", nullable: false),
                    TrialId = table.Column<int>(type: "int", nullable: true),
                    TrialOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    RoleOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetailDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpactTypeId = table.Column<int>(type: "int", nullable: true),
                    RationaleTypeId = table.Column<int>(type: "int", nullable: true),
                    ReasonForChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSubject = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.ChangeRequestId);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ChangeRequestStatuses_StatusChangeRequestStatusId",
                        column: x => x.StatusChangeRequestStatusId,
                        principalTable: "ChangeRequestStatuses",
                        principalColumn: "ChangeRequestStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ImpactTypes_ImpactTypeId",
                        column: x => x.ImpactTypeId,
                        principalTable: "ImpactTypes",
                        principalColumn: "ImpactTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_RationaleTypes_RationaleTypeId",
                        column: x => x.RationaleTypeId,
                        principalTable: "RationaleTypes",
                        principalColumn: "RationaleTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_Trials_TrialId",
                        column: x => x.TrialId,
                        principalTable: "Trials",
                        principalColumn: "TrialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestSignatures",
                columns: table => new
                {
                    ChangeRequestsChangeRequestId = table.Column<int>(type: "int", nullable: false),
                    SignaturesSignatureUploadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestSignatures", x => new { x.ChangeRequestsChangeRequestId, x.SignaturesSignatureUploadId });
                    table.ForeignKey(
                        name: "FK_ChangeRequestSignatures_ChangeRequests_ChangeRequestsChangeRequestId",
                        column: x => x.ChangeRequestsChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestSignatures_SignatureUploads_SignaturesSignatureUploadId",
                        column: x => x.SignaturesSignatureUploadId,
                        principalTable: "SignatureUploads",
                        principalColumn: "SignatureUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevLeadChangeAuthorisations",
                columns: table => new
                {
                    DevLeadChangeAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimatedTimeImpact = table.Column<double>(type: "float", nullable: false),
                    ChangeRequiredDecription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionTypeId = table.Column<int>(type: "int", nullable: true),
                    DecisionExplanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLeadChangeAuthorisations", x => x.DevLeadChangeAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevLeadChangeAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevLeadChangeAuthorisations_DecisionTypes_DecisionTypeId",
                        column: x => x.DecisionTypeId,
                        principalTable: "DecisionTypes",
                        principalColumn: "DecisionTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkCompleteAuthorisations",
                columns: table => new
                {
                    DevWorkCompleteAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommitReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualTimeImpactDays = table.Column<double>(type: "float", nullable: false),
                    ChangeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkCompleteAuthorisations", x => x.DevWorkCompleteAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkCompleteAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevWorkCompleteAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId",
                        column: x => x.ProcessDeviationReasonId,
                        principalTable: "ProcessDeviationReasons",
                        principalColumn: "ProcessDeviationReasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkReleaseAuthorisations",
                columns: table => new
                {
                    DevWorkReleaseAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleasedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReleaseAuthorisations", x => x.DevWorkReleaseAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkReleaseAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevWorkReleaseAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId",
                        column: x => x.ProcessDeviationReasonId,
                        principalTable: "ProcessDeviationReasons",
                        principalColumn: "ProcessDeviationReasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkReviewAuthorisations",
                columns: table => new
                {
                    DevWorkReviewAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReviewAuthorisations", x => x.DevWorkReviewAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId",
                        column: x => x.ProcessDeviationReasonId,
                        principalTable: "ProcessDeviationReasons",
                        principalColumn: "ProcessDeviationReasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    FileUploadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadableFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileUploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.FileUploadId);
                    table.ForeignKey(
                        name: "FK_FileUploads_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeenDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "NotificationTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    RejectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RejectionReasonTypeId = table.Column<int>(type: "int", nullable: true),
                    IsRejectionActive = table.Column<bool>(type: "bit", nullable: false),
                    RejectedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => x.RejectionId);
                    table.ForeignKey(
                        name: "FK_Rejections_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rejections_RejectionReasonTypes_RejectionReasonTypeId",
                        column: x => x.RejectionReasonTypeId,
                        principalTable: "RejectionReasonTypes",
                        principalColumn: "RejectionReasonTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DevLeadAuthSignatures",
                columns: table => new
                {
                    SignaturesSignatureUploadId = table.Column<int>(type: "int", nullable: false),
                    devLeadChangeAuthorisationsDevLeadChangeAuthorisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLeadAuthSignatures", x => new { x.SignaturesSignatureUploadId, x.devLeadChangeAuthorisationsDevLeadChangeAuthorisationId });
                    table.ForeignKey(
                        name: "FK_DevLeadAuthSignatures_DevLeadChangeAuthorisations_devLeadChangeAuthorisationsDevLeadChangeAuthorisationId",
                        column: x => x.devLeadChangeAuthorisationsDevLeadChangeAuthorisationId,
                        principalTable: "DevLeadChangeAuthorisations",
                        principalColumn: "DevLeadChangeAuthorisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevLeadAuthSignatures_SignatureUploads_SignaturesSignatureUploadId",
                        column: x => x.SignaturesSignatureUploadId,
                        principalTable: "SignatureUploads",
                        principalColumn: "SignatureUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkCompleteAuthSignatures",
                columns: table => new
                {
                    SignaturesSignatureUploadId = table.Column<int>(type: "int", nullable: false),
                    devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkCompleteAuthSignatures", x => new { x.SignaturesSignatureUploadId, x.devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId });
                    table.ForeignKey(
                        name: "FK_DevWorkCompleteAuthSignatures_DevWorkCompleteAuthorisations_devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId",
                        column: x => x.devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId,
                        principalTable: "DevWorkCompleteAuthorisations",
                        principalColumn: "DevWorkCompleteAuthorisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevWorkCompleteAuthSignatures_SignatureUploads_SignaturesSignatureUploadId",
                        column: x => x.SignaturesSignatureUploadId,
                        principalTable: "SignatureUploads",
                        principalColumn: "SignatureUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkReleaseAuthSignatures",
                columns: table => new
                {
                    SignaturesSignatureUploadId = table.Column<int>(type: "int", nullable: false),
                    devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReleaseAuthSignatures", x => new { x.SignaturesSignatureUploadId, x.devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId });
                    table.ForeignKey(
                        name: "FK_DevWorkReleaseAuthSignatures_DevWorkReleaseAuthorisations_devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId",
                        column: x => x.devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId,
                        principalTable: "DevWorkReleaseAuthorisations",
                        principalColumn: "DevWorkReleaseAuthorisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevWorkReleaseAuthSignatures_SignatureUploads_SignaturesSignatureUploadId",
                        column: x => x.SignaturesSignatureUploadId,
                        principalTable: "SignatureUploads",
                        principalColumn: "SignatureUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevWorkReviewAuthSignatures",
                columns: table => new
                {
                    SignaturesSignatureUploadId = table.Column<int>(type: "int", nullable: false),
                    devWorkReviewAuthorisationsDevWorkReviewAuthorisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReviewAuthSignatures", x => new { x.SignaturesSignatureUploadId, x.devWorkReviewAuthorisationsDevWorkReviewAuthorisationId });
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthSignatures_DevWorkReviewAuthorisations_devWorkReviewAuthorisationsDevWorkReviewAuthorisationId",
                        column: x => x.devWorkReviewAuthorisationsDevWorkReviewAuthorisationId,
                        principalTable: "DevWorkReviewAuthorisations",
                        principalColumn: "DevWorkReviewAuthorisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthSignatures_SignatureUploads_SignaturesSignatureUploadId",
                        column: x => x.SignaturesSignatureUploadId,
                        principalTable: "SignatureUploads",
                        principalColumn: "SignatureUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SentEmails",
                columns: table => new
                {
                    SentEmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailTypeId = table.Column<int>(type: "int", nullable: true),
                    EmailSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlainTextBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentEmails", x => x.SentEmailId);
                    table.ForeignKey(
                        name: "FK_SentEmails_EmailTypes_EmailTypeId",
                        column: x => x.EmailTypeId,
                        principalTable: "EmailTypes",
                        principalColumn: "EmailTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SentEmails_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ChangeRequestStatuses",
                columns: new[] { "ChangeRequestStatusId", "ChangeRequestStatusName", "IsActive" },
                values: new object[,]
                {
                    { 10, "Pending Requester", true },
                    { 20, "Pending Initial Approval", true },
                    { 30, "Pending Development Work", true },
                    { 40, "Pending Peer Review", true },
                    { 50, "Pending Release", true },
                    { 60, "Completed", true },
                    { 70, "Abandoned", true }
                });

            migrationBuilder.InsertData(
                table: "DecisionTypes",
                columns: new[] { "DecisionTypeId", "DecisionTypeName", "IsActive" },
                values: new object[,]
                {
                    { 10, "Approved", true },
                    { 20, "Rejected with Amendments", true },
                    { 30, "Rejected and Abandoned", true }
                });

            migrationBuilder.InsertData(
                table: "EmailTypes",
                columns: new[] { "EmailTypeId", "EmailTypeName", "IsActive" },
                values: new object[,]
                {
                    { 301, "ChangeRequestPeerReviewDeclinedToDev", true },
                    { 300, "ChangeRequestPeerReviewApprovalToDev", true },
                    { 201, "ChangeRequestPendingPeerReviewToDevTeam", true },
                    { 200, "ChangeRequestPendingDevWorkToDevTeam", true },
                    { 101, "ChangeRequestPendingSubsequentApprovalToDevLead", true },
                    { 202, "ChangeRequestPendingReleaseToDevTeam", true },
                    { 4, "ChangeRequestCompletedToRequester", true },
                    { 3, "ChangeRequestDeclinedAndAbandonedToRequester", true },
                    { 2, "ChangeRequestDeclinedWithAmendmentsToRequester", true },
                    { 1, "ChangeRequestApprovedToRequester", true },
                    { 100, "ChangeRequestPendingInitialApprovalToDevLead", true }
                });

            migrationBuilder.InsertData(
                table: "ImpactTypes",
                columns: new[] { "ImpactTypeId", "ImpactTypeName", "IsActive" },
                values: new object[,]
                {
                    { 10, "Critical", true },
                    { 20, "High", true },
                    { 30, "Medium", true },
                    { 40, "Low", true }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "IsActive", "NotificationTypeName" },
                values: new object[,]
                {
                    { 301, true, "Change Request Pending Peer Review Declined" },
                    { 300, true, "Change Request Pending Peer Review" },
                    { 202, true, "Change Request Pending Release" },
                    { 201, true, "Change Request Pending Peer Review" },
                    { 200, true, "Change Request Pending Development Work" },
                    { 2, true, "Change Request Declined with Amendments" },
                    { 100, true, "Change Request Pending Initial Approval" },
                    { 4, true, "Change Request Completed" },
                    { 3, true, "Change Request Decline and Abandoned" },
                    { 1, true, "Change Request Approved" },
                    { 101, true, "Change Request Pending Subsequent Approval" }
                });

            migrationBuilder.InsertData(
                table: "RationaleTypes",
                columns: new[] { "RationaleTypeId", "IsActive", "RationaleTypeName" },
                values: new object[,]
                {
                    { 4, true, "New Feature" },
                    { 3, true, "Missing Feature" },
                    { 1, true, "Change" },
                    { 2, true, "Bug" }
                });

            migrationBuilder.InsertData(
                table: "RejectionReasonTypes",
                columns: new[] { "RejectionReasonTypeId", "CausesArchive", "IsActive", "RejectionReasonTypeName" },
                values: new object[,]
                {
                    { 1, false, true, "More Detail Needed" },
                    { 2, false, true, "Incorrect Details" }
                });

            migrationBuilder.InsertData(
                table: "RejectionReasonTypes",
                columns: new[] { "RejectionReasonTypeId", "CausesArchive", "IsActive", "RejectionReasonTypeName" },
                values: new object[,]
                {
                    { 100, true, true, "User Error" },
                    { 101, true, true, "Duplicate Request" },
                    { 102, true, true, "Already Fixed" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { 1000, true, "Other" },
                    { 1, true, "Trial Coordinator" },
                    { 2, true, "Research Staff" },
                    { 3, true, "Chief Investigator" },
                    { 4, true, "Data Administrator" }
                });

            migrationBuilder.InsertData(
                table: "Trials",
                columns: new[] { "TrialId", "IsActive", "TrialName" },
                values: new object[,]
                {
                    { 15, true, "PACT" },
                    { 16, true, "ProFHER-2" },
                    { 17, true, "SHEDSSc" },
                    { 18, true, "SNAP2" },
                    { 23, true, "VenUS6" },
                    { 20, true, "_SOPManager" },
                    { 21, true, "STEPFORWARD" },
                    { 22, true, "SWHSI-II" },
                    { 14, true, "OSTRICH" },
                    { 19, true, "SOFFT" },
                    { 13, true, "MODS" },
                    { 7, true, "DIAMONDS" },
                    { 11, true, "GYY" },
                    { 10, true, "Firefli" },
                    { 9, true, "_ETMA" },
                    { 8, true, "DISC [CTIMP]" },
                    { 6, true, "_ChangeRequest" },
                    { 5, true, "BRIGHT" },
                    { 4, true, "BATH-OUT-2" },
                    { 3, true, "BASIL" },
                    { 2, true, "ASSSIST-2" },
                    { 1, true, "ACTIVE" },
                    { 12, true, "L1FE" },
                    { 1000, true, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_ImpactTypeId",
                table: "ChangeRequests",
                column: "ImpactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_RationaleTypeId",
                table: "ChangeRequests",
                column: "RationaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_RoleId",
                table: "ChangeRequests",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_StatusChangeRequestStatusId",
                table: "ChangeRequests",
                column: "StatusChangeRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_TrialId",
                table: "ChangeRequests",
                column: "TrialId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestSignatures_SignaturesSignatureUploadId",
                table: "ChangeRequestSignatures",
                column: "SignaturesSignatureUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_DevLeadAuthSignatures_devLeadChangeAuthorisationsDevLeadChangeAuthorisationId",
                table: "DevLeadAuthSignatures",
                column: "devLeadChangeAuthorisationsDevLeadChangeAuthorisationId");

            migrationBuilder.CreateIndex(
                name: "IX_DevLeadChangeAuthorisations_ChangeRequestId",
                table: "DevLeadChangeAuthorisations",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DevLeadChangeAuthorisations_DecisionTypeId",
                table: "DevLeadChangeAuthorisations",
                column: "DecisionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkCompleteAuthorisations_ChangeRequestId",
                table: "DevWorkCompleteAuthorisations",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkCompleteAuthorisations_ProcessDeviationReasonId",
                table: "DevWorkCompleteAuthorisations",
                column: "ProcessDeviationReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkCompleteAuthSignatures_devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId",
                table: "DevWorkCompleteAuthSignatures",
                column: "devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReleaseAuthorisations_ChangeRequestId",
                table: "DevWorkReleaseAuthorisations",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReleaseAuthorisations_ProcessDeviationReasonId",
                table: "DevWorkReleaseAuthorisations",
                column: "ProcessDeviationReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReleaseAuthSignatures_devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId",
                table: "DevWorkReleaseAuthSignatures",
                column: "devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReviewAuthorisations_ChangeRequestId",
                table: "DevWorkReviewAuthorisations",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReviewAuthorisations_ProcessDeviationReasonId",
                table: "DevWorkReviewAuthorisations",
                column: "ProcessDeviationReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DevWorkReviewAuthSignatures_devWorkReviewAuthorisationsDevWorkReviewAuthorisationId",
                table: "DevWorkReviewAuthSignatures",
                column: "devWorkReviewAuthorisationsDevWorkReviewAuthorisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_ChangeRequestId",
                table: "FileUploads",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ChangeRequestId",
                table: "Notifications",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_ChangeRequestId",
                table: "Rejections",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_RejectionReasonTypeId",
                table: "Rejections",
                column: "RejectionReasonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SentEmails_EmailTypeId",
                table: "SentEmails",
                column: "EmailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SentEmails_NotificationId",
                table: "SentEmails",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChangeRequestSignatures");

            migrationBuilder.DropTable(
                name: "DevLeadAuthSignatures");

            migrationBuilder.DropTable(
                name: "DevWorkCompleteAuthSignatures");

            migrationBuilder.DropTable(
                name: "DevWorkReleaseAuthSignatures");

            migrationBuilder.DropTable(
                name: "DevWorkReviewAuthSignatures");

            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "SentEmails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DevLeadChangeAuthorisations");

            migrationBuilder.DropTable(
                name: "DevWorkCompleteAuthorisations");

            migrationBuilder.DropTable(
                name: "DevWorkReleaseAuthorisations");

            migrationBuilder.DropTable(
                name: "DevWorkReviewAuthorisations");

            migrationBuilder.DropTable(
                name: "SignatureUploads");

            migrationBuilder.DropTable(
                name: "RejectionReasonTypes");

            migrationBuilder.DropTable(
                name: "EmailTypes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "DecisionTypes");

            migrationBuilder.DropTable(
                name: "ProcessDeviationReasons");

            migrationBuilder.DropTable(
                name: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "ChangeRequestStatuses");

            migrationBuilder.DropTable(
                name: "ImpactTypes");

            migrationBuilder.DropTable(
                name: "RationaleTypes");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Trials");
        }
    }
}
