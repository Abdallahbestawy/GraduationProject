using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentResult]
                    @UserId NVARCHAR(450)
                AS
                BEGIN
                    SELECT 
                        u.NameEnglish, 
                        sd.Name AS SemesterName,
                        CONCAT(YEAR(ay.Start), '-', YEAR(ay.[End])) AS AcademyYear,
                        parent.Name AS BandName,
                        CASE
                            WHEN ss.Passing = 1 AND ss.Total IS NOT NULL THEN 'Succeed'
                            WHEN ss.Passing = 0 AND ss.Total IS NOT NULL THEN 'Failed'
                            ELSE 'Waiting'
                        END AS SemesterStatus,
                        cs.Name AS CourseName,
                        cs.Code AS CourseCode,
                        COALESCE(cs.NumberOfPoints, cs.NumberOfCreditHours) AS NumberOfPoints,
                        CASE
                            WHEN ssc.CourseDegree IS NOT NULL THEN CAST(ssc.CourseDegree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS CourseDegree,
                        CASE
                            WHEN ssc.Char IS NOT NULL THEN ssc.Char
                            ELSE 'Waiting'
                        END AS CourseChar,
                        CASE
                            WHEN ssc.Passing = 1 THEN 'Succeed'
                            WHEN ssc.Passing = 0 AND ssc.CourseDegree IS NOT NULL THEN 'Failed'
                            ELSE 'Waiting'
                        END AS CourseStatus,
                        R.Percentage AS SemesterPercentage,
                        R.Char AS SemesterChar,
                        R.PercentageTotal AS CumulativePercentage,
                        R.CharTotal AS CumulativeChar
                    FROM  
                        AspNetUsers AS u  
                    JOIN 
                        Students AS s ON u.Id = s.UserId
                    JOIN 
                        StudentSemesters AS ss ON s.Id = ss.StudentId
                    JOIN 
                        AcademyYears AS ay ON ss.AcademyYearId = ay.Id
                    JOIN 
                        ScientificDegrees AS sd ON ss.ScientificDegreeId = sd.Id
                    LEFT JOIN 
                        ScientificDegrees AS parent ON sd.ParentId = parent.Id
                    JOIN
                        StudentSemesterCourses AS ssc ON ssc.StudentSemesterId = ss.Id
                    JOIN 
                        Courses AS cs ON cs.Id = ssc.CourseId
                    LEFT JOIN
                        Results AS R ON R.StudentSemesterId = ss.Id
                    WHERE 
                        u.Id = @UserId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentResult]");
        }
    }
}
