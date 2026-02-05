using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class GitHubRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GitHubIssueNumber",
                table: "WorkRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GitHubRepositoryId",
                table: "Trials",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubIssueNumber",
                table: "WorkRequests");

            migrationBuilder.DropColumn(
                name: "GitHubRepositoryId",
                table: "Trials");
        }
    }
}
