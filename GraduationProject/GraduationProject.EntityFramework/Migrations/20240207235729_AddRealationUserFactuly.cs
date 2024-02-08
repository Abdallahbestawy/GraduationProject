using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddRealationUserFactuly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Facultys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Facultys",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Facultys_UserId",
                table: "Facultys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facultys_AspNetUsers_UserId",
                table: "Facultys",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facultys_AspNetUsers_UserId",
                table: "Facultys");

            migrationBuilder.DropIndex(
                name: "IX_Facultys_UserId",
                table: "Facultys");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Facultys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Facultys");
        }
    }
}
