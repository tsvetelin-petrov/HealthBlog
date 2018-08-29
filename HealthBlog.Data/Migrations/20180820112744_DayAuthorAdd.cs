using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthBlog.Data.Migrations
{
    public partial class DayAuthorAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Days",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Days_AuthorId",
                table: "Days",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_AspNetUsers_AuthorId",
                table: "Days",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_AspNetUsers_AuthorId",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_AuthorId",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Days");
        }
    }
}
