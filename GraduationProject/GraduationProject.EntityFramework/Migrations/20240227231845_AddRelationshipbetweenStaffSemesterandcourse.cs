using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddRelationshipbetweenStaffSemesterandcourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffSemesters_ScientificDegrees_ScientificDegreeId",
                table: "StaffSemesters");

            migrationBuilder.RenameColumn(
                name: "ScientificDegreeId",
                table: "StaffSemesters",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_StaffSemesters_ScientificDegreeId",
                table: "StaffSemesters",
                newName: "IX_StaffSemesters_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSemesters_Courses_CourseId",
                table: "StaffSemesters",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffSemesters_Courses_CourseId",
                table: "StaffSemesters");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "StaffSemesters",
                newName: "ScientificDegreeId");

            migrationBuilder.RenameIndex(
                name: "IX_StaffSemesters_CourseId",
                table: "StaffSemesters",
                newName: "IX_StaffSemesters_ScientificDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSemesters_ScientificDegrees_ScientificDegreeId",
                table: "StaffSemesters",
                column: "ScientificDegreeId",
                principalTable: "ScientificDegrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
