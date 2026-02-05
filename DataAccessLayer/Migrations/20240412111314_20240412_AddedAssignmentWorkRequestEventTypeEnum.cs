using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class _20240412_AddedAssignmentWorkRequestEventTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkRequestEventTypes",
                columns: new[] { "WorkRequestEventTypeId", "IsActive", "WorkRequestEventTypeName" },
                values: new object[] { 10, false, "Assignment" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 10);
        }
    }
}
