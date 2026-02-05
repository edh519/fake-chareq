using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscribeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkRequestEventTypes",
                columns: new[] { "WorkRequestEventTypeId", "IsActive", "WorkRequestEventTypeName" },
                values: new object[] { 24, false, "Subscribe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkRequestEventTypes",
                keyColumn: "WorkRequestEventTypeId",
                keyValue: 24);
        }
    }
}
