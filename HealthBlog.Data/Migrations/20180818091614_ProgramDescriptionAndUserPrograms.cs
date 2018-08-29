using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class ProgramDescriptionAndUserPrograms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProgramDays");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Programs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Programs");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProgramDays",
                nullable: true);
        }
    }
}
