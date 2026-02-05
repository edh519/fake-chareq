using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageNotificationEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "IsActive", "NotificationTypeName" },
                values: new object[] { 300, true, "New Message" });

            migrationBuilder.UpdateData(
                table: "WorkRequestStatuses",
                keyColumn: "WorkRequestStatusId",
                keyValue: 110,
                column: "WorkRequestStatusName",
                value: "Closed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 300);

            migrationBuilder.UpdateData(
                table: "WorkRequestStatuses",
                keyColumn: "WorkRequestStatusId",
                keyValue: 110,
                column: "WorkRequestStatusName",
                value: "Abandoned");
        }
    }
}
