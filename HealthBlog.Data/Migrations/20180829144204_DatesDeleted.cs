using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class DatesDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplited",
                table: "TrainingDays");

            migrationBuilder.DropColumn(
                name: "TrainingTime",
                table: "TrainingDays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplited",
                table: "TrainingDays",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrainingTime",
                table: "TrainingDays",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
