using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddedLastEditedDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditedDateTime",
                table: "WorkRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditedDateTime",
                table: "WorkRequests")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "WorkRequestsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
