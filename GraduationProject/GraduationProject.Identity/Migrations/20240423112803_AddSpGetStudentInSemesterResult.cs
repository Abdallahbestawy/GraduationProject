using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentInSemesterResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentInSemesterResult]
                    @StudentSemesterId INT
                AS
                BEGIN
                    SELECT 
                        std.Code AS StudentCode,
                        ui.NameEnglish AS StudentName,
                        c.Code AS CourseName,
                        c.Name AS CourseCode,
                        COALESCE(c.NumberOfPoints, c.NumberOfCreditHours) AS NumberOfPoints,
                        AssessMethods.Name AS AssessMethodsName,
                        CASE
                            WHEN StudentSemesterAssessMethods.Degree IS NOT NULL THEN CAST(StudentSemesterAssessMethods.Degree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS Degree,
                        CASE
                            WHEN ssc.Char IS NOT NULL THEN ssc.Char
                            ELSE 'Waiting'
                        END AS CourseChar,
                        CASE
                            WHEN ssc.CourseDegree IS NOT NULL THEN CAST(ssc.CourseDegree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS CourseDegree,
                        CASE
                            WHEN ss.Passing = 1 AND ss.Total IS NOT NULL THEN 'Succeed'
                            WHEN ss.Passing = 0 AND ss.Total IS NOT NULL THEN 'Failed'
                            ELSE 'Waiting'
                        END AS SemesterStatus,
                        CASE
                            WHEN ssc.Passing = 1 THEN 'Succeed'
                            WHEN ssc.Passing = 0 AND ssc.CourseDegree IS NOT NULL THEN 'Failed'
                            ELSE 'Waiting'
                        END AS CourseStatus,
                        R.Percentage AS StudentSemesterPercentage,
                        R.Char AS StudentSemesterChar,
                        R.PercentageTotal AS StudentCumulativePercentage,
                        R.CharTotal AS StudentCumulativeChar
                    FROM 
                        StudentSemesters AS ss
                    JOIN 
                        Students AS std ON std.Id = ss.StudentId
                    JOIN
                        AspNetUsers AS ui ON ui.Id = std.UserId
                    JOIN 
                        StudentSemesterCourses AS ssc ON ssc.StudentSemesterId = ss.Id
                    JOIN 
                        Courses AS c ON c.Id = ssc.CourseId
                    JOIN 
                        CourseAssessMethods ON CourseAssessMethods.CourseId = c.Id
                    JOIN 
                        AssessMethods ON AssessMethods.Id = CourseAssessMethods.AssessMethodId
                    JOIN 
                        StudentSemesterAssessMethods ON StudentSemesterAssessMethods.CourseAssessMethodId = CourseAssessMethods.Id
                    LEFT JOIN 
                        Results AS R ON R.StudentSemesterId = ss.Id
                    WHERE 
                        ss.Id = @StudentSemesterId 
                        AND ss.Id = StudentSemesterAssessMethods.StudentSemesterId;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentInSemesterResult]");
        }
    }
}
