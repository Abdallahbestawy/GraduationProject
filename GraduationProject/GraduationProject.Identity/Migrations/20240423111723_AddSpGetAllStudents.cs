using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetAllStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetAllStudents]
                    @FacultyId INT
                AS
                BEGIN
                    SELECT 
                        Students.Id,
                        Students.UserId,
                        AspNetUsers.NameArabic,
                        AspNetUsers.NameEnglish,
                        AspNetUsers.Email,
                        Students.Gender,
                        Students.Nationality,
                        Students.Religion
                    FROM
                        Students
                    JOIN
                        AspNetUsers ON Students.UserId = AspNetUsers.Id
                    WHERE
                        Students.FacultyId = @FacultyId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetAllStudents]");
        }
    }
}
