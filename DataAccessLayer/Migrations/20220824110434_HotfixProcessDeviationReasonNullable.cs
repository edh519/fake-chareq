using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class HotfixProcessDeviationReasonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "ProcessDeviationReasonId",
                table: "FinalAuthorisations",
                nullable: true,
                type: "int"
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProcessDeviationReasonId",
                table: "FinalAuthorisations",
                nullable: false,
                type: "int"
                );
        }
    }
}
