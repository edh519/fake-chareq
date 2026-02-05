using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //To avoid conflicts, demote all workers to users
            migrationBuilder.Sql(@"UPDATE dbo.AspNetUserRoles SET RoleId = 71 WHERE RoleId = 53");

            //Once all workers have been demoted, delete the worker role
            migrationBuilder.Sql(@"DELETE FROM dbo.AspNetRoles WHERE Id = 53");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the deleted role (RoleId = 53)
            migrationBuilder.Sql(@"
                INSERT INTO dbo.AspNetRoles (Id, [Name], [NormalizedName], [ConcurrencyStamp])
                VALUES (53, 'Worker', 'WORKER', NEWID())
            ");

            // Don't reassign users back to the Worker role as we cannot be sure who had that role before
        }
    }
}
