using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddNotesInTapleStudentSemesterCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "StudentSemesterCourses",
                type: "nvarchar(max)",
                nullable: true);




        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "StudentSemesterCourses");


        }
    }
}
