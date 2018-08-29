using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class ProgramAndUserProgramChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Programs");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserPrograms",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Programs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Programs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_AuthorId",
                table: "Programs",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_AspNetUsers_AuthorId",
                table: "Programs",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_AspNetUsers_AuthorId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_AuthorId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserPrograms");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Programs");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Programs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
