using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class ProcessDeviationReason_WorkRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessDeviationReasonId",
                table: "WorkRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkRequests_ProcessDeviationReasonId",
                table: "WorkRequests",
                column: "ProcessDeviationReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRequests_ProcessDeviationReasons_ProcessDeviationReasonId",
                table: "WorkRequests",
                column: "ProcessDeviationReasonId",
                principalTable: "ProcessDeviationReasons",
                principalColumn: "ProcessDeviationReasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkRequests_ProcessDeviationReasons_ProcessDeviationReasonId",
                table: "WorkRequests");

            migrationBuilder.DropIndex(
                name: "IX_WorkRequests_ProcessDeviationReasonId",
                table: "WorkRequests");

            migrationBuilder.DropColumn(
                name: "ProcessDeviationReasonId",
                table: "WorkRequests")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "WorkRequestsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
