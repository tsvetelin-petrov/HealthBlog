using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class UserTrainerChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrainer",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CertificateUploadTimes",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateUploadTimes",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrainer",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
