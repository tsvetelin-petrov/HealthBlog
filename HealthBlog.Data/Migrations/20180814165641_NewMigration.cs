using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Programs_ProgramId1",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_Date",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_ProgramId1",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "ProgramId1",
                table: "Days");

            migrationBuilder.CreateTable(
                name: "ProgramDays",
                columns: table => new
                {
                    ProgramId = table.Column<int>(nullable: false),
                    DayId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDays", x => new { x.DayId, x.ProgramId });
                    table.ForeignKey(
                        name: "FK_ProgramDays_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramDays_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDays_ProgramId",
                table: "ProgramDays",
                column: "ProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramDays");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Programs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Days",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProgramId",
                table: "Days",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramId1",
                table: "Days",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Days_Date",
                table: "Days",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Days_ProgramId1",
                table: "Days",
                column: "ProgramId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Programs_ProgramId1",
                table: "Days",
                column: "ProgramId1",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
