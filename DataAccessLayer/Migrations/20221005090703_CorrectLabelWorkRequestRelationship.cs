using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class CorrectLabelWorkRequestRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_WorkRequests_WorkRequestId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_WorkRequestId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "WorkRequestId",
                table: "Label");

            migrationBuilder.CreateTable(
                name: "LabelWorkRequest",
                columns: table => new
                {
                    LabelsLabelId = table.Column<int>(type: "int", nullable: false),
                    WorkRequestsWorkRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelWorkRequest", x => new { x.LabelsLabelId, x.WorkRequestsWorkRequestId });
                    table.ForeignKey(
                        name: "FK_LabelWorkRequest_Label_LabelsLabelId",
                        column: x => x.LabelsLabelId,
                        principalTable: "Label",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelWorkRequest_WorkRequests_WorkRequestsWorkRequestId",
                        column: x => x.WorkRequestsWorkRequestId,
                        principalTable: "WorkRequests",
                        principalColumn: "WorkRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelWorkRequest_WorkRequestsWorkRequestId",
                table: "LabelWorkRequest",
                column: "WorkRequestsWorkRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelWorkRequest");

            migrationBuilder.AddColumn<int>(
                name: "WorkRequestId",
                table: "Label",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Label_WorkRequestId",
                table: "Label",
                column: "WorkRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_WorkRequests_WorkRequestId",
                table: "Label",
                column: "WorkRequestId",
                principalTable: "WorkRequests",
                principalColumn: "WorkRequestId");
        }
    }
}
