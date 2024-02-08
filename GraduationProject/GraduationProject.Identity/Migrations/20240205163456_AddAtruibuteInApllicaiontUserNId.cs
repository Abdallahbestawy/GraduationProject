using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddAtruibuteInApllicaiontUserNId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalID",
                table: "AspNetUsers",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalID",
                table: "AspNetUsers");
        }
    }
}
