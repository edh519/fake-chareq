using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTrialRepositoryInfoMultiRepoConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubRepositoryId",
                table: "Trials")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TrialsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<long>(
                name: "AssignedTrialRepositoryId",
                table: "WorkRequests",
                type: "bigint",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "WorkRequestsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "TrialRepositoryInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    GitHubRepositoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    TrialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrialRepositoryInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrialRepositoryInfos_Trials_TrialId",
                        column: x => x.TrialId,
                        principalTable: "Trials",
                        principalColumn: "TrialId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");


            migrationBuilder.CreateIndex(
                name: "IX_TrialRepositoryInfos_TrialId",
                table: "TrialRepositoryInfos",
                column: "TrialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrialRepositoryInfos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TrialRepositoryInfosHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "AssignedTrialRepositoryId",
                table: "WorkRequests")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "WorkRequestsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<long>(
                name: "GitHubRepositoryId",
                table: "Trials",
                type: "bigint",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TrialsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 1,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 2,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 3,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 4,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 5,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 6,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 7,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 8,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 9,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 10,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 11,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 12,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 13,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 14,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 15,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 16,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 17,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 18,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 19,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 20,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 21,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 22,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 23,
                column: "GitHubRepositoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trials",
                keyColumn: "TrialId",
                keyValue: 1000,
                column: "GitHubRepositoryId",
                value: null);

        }
    }
}
