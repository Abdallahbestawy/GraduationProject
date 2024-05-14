using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddFactulyIdINstudentAndStaffNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Staffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyId",
                table: "Students",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_FacultyId",
                table: "Staffs",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FacultyId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_FacultyId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Staffs");
        }
    }
}
