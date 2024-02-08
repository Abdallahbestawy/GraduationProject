using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddAtruibuteInApllicaiontUserNameArabicAndEnglish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameArabic",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEnglish",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameArabic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NameEnglish",
                table: "AspNetUsers");
        }
    }
}
