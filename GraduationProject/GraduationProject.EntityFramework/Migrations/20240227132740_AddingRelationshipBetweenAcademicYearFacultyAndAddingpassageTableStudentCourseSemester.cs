using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddingRelationshipBetweenAcademicYearFacultyAndAddingpassageTableStudentCourseSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CourseDegree",
                table: "StudentSemesterCourses",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "Passing",
                table: "StudentSemesterCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "AcademyYears",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AcademyYears_FacultyId",
                table: "AcademyYears",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademyYears_Facultys_FacultyId",
                table: "AcademyYears",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademyYears_Facultys_FacultyId",
                table: "AcademyYears");

            migrationBuilder.DropIndex(
                name: "IX_AcademyYears_FacultyId",
                table: "AcademyYears");

            migrationBuilder.DropColumn(
                name: "Passing",
                table: "StudentSemesterCourses");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "AcademyYears");

            migrationBuilder.AlterColumn<decimal>(
                name: "CourseDegree",
                table: "StudentSemesterCourses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
