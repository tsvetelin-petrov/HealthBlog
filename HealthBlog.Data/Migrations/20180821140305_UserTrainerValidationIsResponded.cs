using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class UserTrainerValidationIsResponded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserPrograms");

            migrationBuilder.RenameColumn(
                name: "CertificateUrl",
                table: "AspNetUsers",
                newName: "CertificatePath");

            migrationBuilder.AddColumn<bool>(
                name: "IsResponded",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResponded",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CertificatePath",
                table: "AspNetUsers",
                newName: "CertificateUrl");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserPrograms",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
