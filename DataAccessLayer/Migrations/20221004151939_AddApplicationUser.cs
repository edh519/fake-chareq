using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkRequests_WorkRequestId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkRequestId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkRequestId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserWorkRequest",
                columns: table => new
                {
                    AssigneesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkRequestsWorkRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserWorkRequest", x => new { x.AssigneesId, x.WorkRequestsWorkRequestId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserWorkRequest_AspNetUsers_AssigneesId",
                        column: x => x.AssigneesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserWorkRequest_WorkRequests_WorkRequestsWorkRequestId",
                        column: x => x.WorkRequestsWorkRequestId,
                        principalTable: "WorkRequests",
                        principalColumn: "WorkRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "IsActive", "NotificationTypeName" },
                values: new object[] { 201, true, "Assigned to Request" });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "IsActive", "NotificationTypeName" },
                values: new object[] { 202, true, "Unassigned from Request" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserWorkRequest_WorkRequestsWorkRequestId",
                table: "ApplicationUserWorkRequest",
                column: "WorkRequestsWorkRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserWorkRequest");

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 202);

            migrationBuilder.AddColumn<int>(
                name: "WorkRequestId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkRequestId",
                table: "AspNetUsers",
                column: "WorkRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkRequests_WorkRequestId",
                table: "AspNetUsers",
                column: "WorkRequestId",
                principalTable: "WorkRequests",
                principalColumn: "WorkRequestId");
        }
    }
}
