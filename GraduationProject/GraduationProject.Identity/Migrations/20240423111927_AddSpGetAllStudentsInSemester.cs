using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetAllStudentsInSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetAllStudentsInSemester]
                    @ScientificDegreeId INT
                AS
                BEGIN
                    SELECT
                        AspNetUsers.NameEnglish,
                        Students.Code,
                        StudentSemesters.Id
                    FROM
                        StudentSemesters
                    JOIN
                        Students ON Students.Id = StudentSemesters.StudentId
                    JOIN
                        AspNetUsers ON AspNetUsers.Id = Students.UserId
                    WHERE
                        StudentSemesters.ScientificDegreeId = @ScientificDegreeId;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetAllStudentsInSemester]");
        }
    }
}
