using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class WorkRequestEventTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkRequestEventTypes",
                columns: new[] { "WorkRequestEventTypeId", "IsActive", "WorkRequestEventTypeName" },
                values: new object[,]
                {
                    { 1, true, "Message" },
                    { 2, true, "Requesting Changes" },
                    { 3, true, "Approving" },
                    { 4, true, "Completing" },
                    { 5, true, "Closing" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 5);
        }
    }
}
