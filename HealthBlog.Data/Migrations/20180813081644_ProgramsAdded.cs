using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class ProgramsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_AspNetUsers_UserId",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_UserId",
                table: "Days");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Days",
                newName: "ProgramId");

            migrationBuilder.AlterColumn<string>(
                name: "TargetMuscle",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProgramId",
                table: "Days",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramId1",
                table: "Days",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrainer",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    DurationInDays = table.Column<int>(nullable: false),
                    IsForSale = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPrograms",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    ProgramId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrograms", x => new { x.UserId, x.ProgramId });
                    table.ForeignKey(
                        name: "FK_UserPrograms_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrograms_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Days_ProgramId1",
                table: "Days",
                column: "ProgramId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_ProgramId",
                table: "UserPrograms",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_UserId1",
                table: "UserPrograms",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Programs_ProgramId1",
                table: "Days",
                column: "ProgramId1",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Programs_ProgramId1",
                table: "Days");

            migrationBuilder.DropTable(
                name: "UserPrograms");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Days_ProgramId1",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "ProgramId1",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsTrainer",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Days",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "TargetMuscle",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Days",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Days_UserId",
                table: "Days",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_AspNetUsers_UserId",
                table: "Days",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
