using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetGraduateStudentsByAcademyYearId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetGraduateStudentsByAcademyYearId]
                    @AcademyYearId INT
                AS
                BEGIN
                    SELECT 
                        CONCAT(YEAR(ay.Start), '-', YEAR(ay.[End])) AS AcademyYear,
                        au.NameEnglish AS StudentName,
                        s.Code AS StudentCode,
                        R.PercentageTotal,
                        R.CharTotal
                    FROM 
                        StudentSemesters AS ss 
                    JOIN 
                        AcademyYears AS ay ON ay.Id = ss.AcademyYearId
                    JOIN 
                        Students AS s ON s.Id = ss.StudentId
                    JOIN 
                        AspNetUsers AS au ON au.Id = s.UserId
                    JOIN 
                        Results AS R ON R.StudentSemesterId = ss.Id
                    WHERE 
                        ss.AcademyYearId = @AcademyYearId 
                        AND ss.IsGraduate = 1;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetGraduateStudentsByAcademyYearId]");
        }
    }
}
