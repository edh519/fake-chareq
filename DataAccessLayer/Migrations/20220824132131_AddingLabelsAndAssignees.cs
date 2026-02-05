using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddingLabelsAndAssignees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkRequestId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelShort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HexColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    WorkRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.LabelId);
                    table.ForeignKey(
                        name: "FK_Label_WorkRequests_WorkRequestId",
                        column: x => x.WorkRequestId,
                        principalTable: "WorkRequests",
                        principalColumn: "WorkRequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkRequestId",
                table: "AspNetUsers",
                column: "WorkRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_WorkRequestId",
                table: "Label",
                column: "WorkRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkRequests_WorkRequestId",
                table: "AspNetUsers",
                column: "WorkRequestId",
                principalTable: "WorkRequests",
                principalColumn: "WorkRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkRequests_WorkRequestId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkRequestId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkRequestId",
                table: "AspNetUsers");
        }
    }
}
