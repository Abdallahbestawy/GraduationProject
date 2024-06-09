using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentsSemesterResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentsSemesterResult]
                    @AcademyYearId INT,
                    @ScientificDegreeId INT
                AS
                BEGIN
                    SELECT 
                        s.Code AS StudentCode,
                        sd.Name AS SemesterName,
                        CONCAT(YEAR(ay.Start), '-', YEAR(ay.[End])) AS AcademyYear,
                        parent.Name AS BandName,
                        ui.NameEnglish AS StudentName,
                        cs.Code AS CourseCode,
                        cs.Name AS CourseName,
                        CASE
                            WHEN ssc.CourseDegree IS NOT NULL THEN CAST(ssc.CourseDegree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS CourseDegree,
                        CASE
                            WHEN ssc.Char IS NOT NULL THEN ssc.Char
                            ELSE 'Waiting'
                        END AS CourseChar,
                        COALESCE(cs.NumberOfPoints, cs.NumberOfCreditHours) AS NumberOfPoints,
                        AssessMethods.Name,
                        CASE
                            WHEN StudentSemesterAssessMethods.Degree IS NOT NULL THEN CAST(StudentSemesterAssessMethods.Degree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS Degree,
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
                        AcademyYears AS ay ON ss.AcademyYearId = ay.Id
                    JOIN 
                        ScientificDegrees AS sd ON ss.ScientificDegreeId = sd.Id
                    LEFT JOIN 
                        ScientificDegrees AS parent ON sd.ParentId = parent.Id
                    JOIN 
                        Students AS s ON s.Id = ss.StudentId
                    JOIN 
                        AspNetUsers AS ui ON ui.Id = s.UserId
                    JOIN 
                        StudentSemesterCourses AS ssc ON ssc.StudentSemesterId = ss.Id
                    JOIN 
                        Courses AS cs ON cs.Id = ssc.CourseId
                    JOIN 
                        CourseAssessMethods AS cam ON cam.CourseId = cs.Id
                    JOIN 
                        AssessMethods ON AssessMethods.Id = cam.AssessMethodId
                    JOIN 
                        StudentSemesterAssessMethods ON StudentSemesterAssessMethods.CourseAssessMethodId = cam.Id
                    LEFT JOIN 
                        Results AS R ON R.StudentSemesterId = ss.Id
                    WHERE 
                        (ss.AcademyYearId = @AcademyYearId AND ss.ScientificDegreeId = @ScientificDegreeId) AND
                        (ss.Id = StudentSemesterAssessMethods.StudentSemesterId)
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentsSemesterResult]");
        }
    }
}
