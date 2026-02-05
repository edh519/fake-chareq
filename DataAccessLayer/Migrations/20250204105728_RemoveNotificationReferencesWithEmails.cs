using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationReferencesWithEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SentEmails_Notifications_NotificationId",
                table: "SentEmails");

            migrationBuilder.DropIndex(
                name: "IX_SentEmails_NotificationId",
                table: "SentEmails");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "SentEmails")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SentEmailsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "SentEmails",
                type: "int",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SentEmailsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_SentEmails_NotificationId",
                table: "SentEmails",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SentEmails_Notifications_NotificationId",
                table: "SentEmails",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "NotificationId");
        }
    }
}
