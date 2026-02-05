using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class version2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_ChangeRequests_ChangeRequestId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ChangeRequests_ChangeRequestId",
                table: "Notifications");

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
                name: "Rejections");

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
                name: "DecisionTypes");

            migrationBuilder.DropTable(
                name: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "ChangeRequestStatuses");

            migrationBuilder.DropTable(
                name: "ImpactTypes");

            migrationBuilder.DropTable(
                name: "RationaleTypes");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "EmailTypes",
                keyColumn: "EmailTypeId",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 301);

            migrationBuilder.RenameColumn(
                name: "ChangeRequestId",
                table: "Notifications",
                newName: "WorkRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ChangeRequestId",
                table: "Notifications",
                newName: "IX_Notifications_WorkRequestId");

            migrationBuilder.RenameColumn(
                name: "ChangeRequestId",
                table: "FileUploads",
                newName: "WorkRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FileUploads_ChangeRequestId",
                table: "FileUploads",
                newName: "IX_FileUploads_WorkRequestId");

            migrationBuilder.CreateTable(
                name: "WorkRequestStatuses",
                columns: table => new
                {
                    WorkRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    WorkRequestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRequestStatuses", x => x.WorkRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "WorkRequests",
                columns: table => new
                {
                    WorkRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusWorkRequestStatusId = table.Column<int>(type: "int", nullable: true),
                    TrialId = table.Column<int>(type: "int", nullable: true),
                    TrialOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetailDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRequests", x => x.WorkRequestId);
                    table.ForeignKey(
                        name: "FK_WorkRequests_Trials_TrialId",
                        column: x => x.TrialId,
                        principalTable: "Trials",
                        principalColumn: "TrialId");
                    table.ForeignKey(
                        name: "FK_WorkRequests_WorkRequestStatuses_StatusWorkRequestStatusId",
                        column: x => x.StatusWorkRequestStatusId,
                        principalTable: "WorkRequestStatuses",
                        principalColumn: "WorkRequestStatusId");
                });

            migrationBuilder.CreateTable(
                name: "FinalAuthorisations",
                columns: table => new
                {
                    FinalAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualTimeImpactDays = table.Column<double>(type: "float", nullable: false),
                    CompletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalAuthorisations", x => x.FinalAuthorisationId);
                    table.ForeignKey(
                        name: "FK_FinalAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId",
                        column: x => x.ProcessDeviationReasonId,
                        principalTable: "ProcessDeviationReasons",
                        principalColumn: "ProcessDeviationReasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalAuthorisations_WorkRequests_WorkRequestId",
                        column: x => x.WorkRequestId,
                        principalTable: "WorkRequests",
                        principalColumn: "WorkRequestId");
                });

            migrationBuilder.CreateTable(
                name: "InitialAuthorisations",
                columns: table => new
                {
                    InitialAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimatedTimeImpact = table.Column<double>(type: "float", nullable: false),
                    WorkRequiredDecription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialAuthorisations", x => x.InitialAuthorisationId);
                    table.ForeignKey(
                        name: "FK_InitialAuthorisations_WorkRequests_WorkRequestId",
                        column: x => x.WorkRequestId,
                        principalTable: "WorkRequests",
                        principalColumn: "WorkRequestId");
                });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 1,
                column: "NotificationTypeName",
                value: "Work Request Approved");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 2,
                column: "NotificationTypeName",
                value: "Request Requires Ammendments");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 3,
                column: "NotificationTypeName",
                value: "Request Closed");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 4,
                column: "NotificationTypeName",
                value: "Request Completed");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 100,
                column: "NotificationTypeName",
                value: "Request Pending Initial Approval");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 101,
                column: "NotificationTypeName",
                value: "Request Re-submitted Pending Initial Approval");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 200,
                column: "NotificationTypeName",
                value: "Request Pending Work");

            migrationBuilder.InsertData(
                table: "WorkRequestStatuses",
                columns: new[] { "WorkRequestStatusId", "IsActive", "WorkRequestStatusName" },
                values: new object[,]
                {
                    { 10, true, "Pending Requester" },
                    { 20, true, "Pending Initial Approval" },
                    { 30, true, "Pending Work" },
                    { 100, true, "Completed" },
                    { 110, true, "Abandoned" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuthorisations_ProcessDeviationReasonId",
                table: "FinalAuthorisations",
                column: "ProcessDeviationReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuthorisations_WorkRequestId",
                table: "FinalAuthorisations",
                column: "WorkRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialAuthorisations_WorkRequestId",
                table: "InitialAuthorisations",
                column: "WorkRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRequests_StatusWorkRequestStatusId",
                table: "WorkRequests",
                column: "StatusWorkRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRequests_TrialId",
                table: "WorkRequests",
                column: "TrialId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_WorkRequests_WorkRequestId",
                table: "FileUploads",
                column: "WorkRequestId",
                principalTable: "WorkRequests",
                principalColumn: "WorkRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_WorkRequests_WorkRequestId",
                table: "Notifications",
                column: "WorkRequestId",
                principalTable: "WorkRequests",
                principalColumn: "WorkRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_WorkRequests_WorkRequestId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_WorkRequests_WorkRequestId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "FinalAuthorisations");

            migrationBuilder.DropTable(
                name: "InitialAuthorisations");

            migrationBuilder.DropTable(
                name: "WorkRequests");

            migrationBuilder.DropTable(
                name: "WorkRequestStatuses");

            migrationBuilder.RenameColumn(
                name: "WorkRequestId",
                table: "Notifications",
                newName: "ChangeRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_WorkRequestId",
                table: "Notifications",
                newName: "IX_Notifications_ChangeRequestId");

            migrationBuilder.RenameColumn(
                name: "WorkRequestId",
                table: "FileUploads",
                newName: "ChangeRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FileUploads_WorkRequestId",
                table: "FileUploads",
                newName: "IX_FileUploads_ChangeRequestId");

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
                name: "RationaleTypes",
                columns: table => new
                {
                    RationaleTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RationaleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    CausesArchive = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RejectionReasonTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignatureUploads", x => x.SignatureUploadId);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImpactTypeId = table.Column<int>(type: "int", nullable: true),
                    RationaleTypeId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    StatusChangeRequestStatusId = table.Column<int>(type: "int", nullable: true),
                    TrialId = table.Column<int>(type: "int", nullable: true),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetailDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasActiveRejection = table.Column<bool>(type: "bit", nullable: false),
                    ReasonForChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSubject = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    TrialOther = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.ChangeRequestId);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ChangeRequestStatuses_StatusChangeRequestStatusId",
                        column: x => x.StatusChangeRequestStatusId,
                        principalTable: "ChangeRequestStatuses",
                        principalColumn: "ChangeRequestStatusId");
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ImpactTypes_ImpactTypeId",
                        column: x => x.ImpactTypeId,
                        principalTable: "ImpactTypes",
                        principalColumn: "ImpactTypeId");
                    table.ForeignKey(
                        name: "FK_ChangeRequests_RationaleTypes_RationaleTypeId",
                        column: x => x.RationaleTypeId,
                        principalTable: "RationaleTypes",
                        principalColumn: "RationaleTypeId");
                    table.ForeignKey(
                        name: "FK_ChangeRequests_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_ChangeRequests_Trials_TrialId",
                        column: x => x.TrialId,
                        principalTable: "Trials",
                        principalColumn: "TrialId");
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
                    DecisionTypeId = table.Column<int>(type: "int", nullable: true),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    ChangeRequiredDecription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionExplanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedTimeImpact = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLeadChangeAuthorisations", x => x.DevLeadChangeAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevLeadChangeAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId");
                    table.ForeignKey(
                        name: "FK_DevLeadChangeAuthorisations_DecisionTypes_DecisionTypeId",
                        column: x => x.DecisionTypeId,
                        principalTable: "DecisionTypes",
                        principalColumn: "DecisionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "DevWorkCompleteAuthorisations",
                columns: table => new
                {
                    DevWorkCompleteAuthorisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    ActualTimeImpactDays = table.Column<double>(type: "float", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    CommitReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkCompleteAuthorisations", x => x.DevWorkCompleteAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkCompleteAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId");
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
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    ReleasedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReleaseAuthorisations", x => x.DevWorkReleaseAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkReleaseAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId");
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
                    ProcessDeviationReasonId = table.Column<int>(type: "int", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevWorkReviewAuthorisations", x => x.DevWorkReviewAuthorisationId);
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthorisations_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId");
                    table.ForeignKey(
                        name: "FK_DevWorkReviewAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId",
                        column: x => x.ProcessDeviationReasonId,
                        principalTable: "ProcessDeviationReasons",
                        principalColumn: "ProcessDeviationReasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    RejectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RejectionReasonTypeId = table.Column<int>(type: "int", nullable: true),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: true),
                    IsRejectionActive = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => x.RejectionId);
                    table.ForeignKey(
                        name: "FK_Rejections_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId");
                    table.ForeignKey(
                        name: "FK_Rejections_RejectionReasonTypes_RejectionReasonTypeId",
                        column: x => x.RejectionReasonTypeId,
                        principalTable: "RejectionReasonTypes",
                        principalColumn: "RejectionReasonTypeId");
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
                    { 1, "ChangeRequestApprovedToRequester", true },
                    { 2, "ChangeRequestDeclinedWithAmendmentsToRequester", true },
                    { 3, "ChangeRequestDeclinedAndAbandonedToRequester", true },
                    { 4, "ChangeRequestCompletedToRequester", true },
                    { 100, "ChangeRequestPendingInitialApprovalToDevLead", true },
                    { 101, "ChangeRequestPendingSubsequentApprovalToDevLead", true },
                    { 200, "ChangeRequestPendingDevWorkToDevTeam", true },
                    { 201, "ChangeRequestPendingPeerReviewToDevTeam", true },
                    { 202, "ChangeRequestPendingReleaseToDevTeam", true },
                    { 300, "ChangeRequestPeerReviewApprovalToDev", true },
                    { 301, "ChangeRequestPeerReviewDeclinedToDev", true }
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

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 1,
                column: "NotificationTypeName",
                value: "Change Request Approved");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 2,
                column: "NotificationTypeName",
                value: "Change Request Declined with Amendments");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 3,
                column: "NotificationTypeName",
                value: "Change Request Decline and Abandoned");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 4,
                column: "NotificationTypeName",
                value: "Change Request Completed");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 100,
                column: "NotificationTypeName",
                value: "Change Request Pending Initial Approval");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 101,
                column: "NotificationTypeName",
                value: "Change Request Pending Subsequent Approval");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 200,
                column: "NotificationTypeName",
                value: "Change Request Pending Development Work");

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "IsActive", "NotificationTypeName" },
                values: new object[,]
                {
                    { 201, true, "Change Request Pending Peer Review" },
                    { 202, true, "Change Request Pending Release" },
                    { 300, true, "Change Request Pending Peer Review" },
                    { 301, true, "Change Request Pending Peer Review Declined" }
                });

            migrationBuilder.InsertData(
                table: "RationaleTypes",
                columns: new[] { "RationaleTypeId", "IsActive", "RationaleTypeName" },
                values: new object[,]
                {
                    { 1, true, "Change" },
                    { 2, true, "Bug" },
                    { 3, true, "Missing Feature" },
                    { 4, true, "New Feature" }
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
                    { 1, true, "Trial Coordinator" },
                    { 2, true, "Research Staff" },
                    { 3, true, "Chief Investigator" },
                    { 4, true, "Data Administrator" },
                    { 1000, true, "Other" }
                });

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
                name: "IX_Rejections_ChangeRequestId",
                table: "Rejections",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_RejectionReasonTypeId",
                table: "Rejections",
                column: "RejectionReasonTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_ChangeRequests_ChangeRequestId",
                table: "FileUploads",
                column: "ChangeRequestId",
                principalTable: "ChangeRequests",
                principalColumn: "ChangeRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ChangeRequests_ChangeRequestId",
                table: "Notifications",
                column: "ChangeRequestId",
                principalTable: "ChangeRequests",
                principalColumn: "ChangeRequestId");
        }
    }
}
