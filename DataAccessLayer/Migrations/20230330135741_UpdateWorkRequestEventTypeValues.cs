using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class UpdateWorkRequestEventTypeValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 2,
                column: "WorkRequestEventTypeName",
                value: "Request Changes");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 3,
                column: "WorkRequestEventTypeName",
                value: "Approve");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 4,
                column: "WorkRequestEventTypeName",
                value: "Complete");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 5,
                column: "WorkRequestEventTypeName",
                value: "Close");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 2,
                column: "WorkRequestEventTypeName",
                value: "Requesting Changes");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 3,
                column: "WorkRequestEventTypeName",
                value: "Approving");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 4,
                column: "WorkRequestEventTypeName",
                value: "Completing");

            migrationBuilder.UpdateData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 5,
                column: "WorkRequestEventTypeName",
                value: "Closing");
        }
    }
}
