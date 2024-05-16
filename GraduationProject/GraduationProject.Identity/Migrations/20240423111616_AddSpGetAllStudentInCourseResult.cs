using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetAllStudentInCourseResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetAllStudentInCourseResult]
                    @AcademyYearId INT,
                    @ScientificDegreeId INT,
                    @CourseId INT
                AS
                BEGIN
                    SELECT 
                        Students.Code AS StudentCode,
                        ui.NameEnglish AS StudentName,
                        c.Name AS CourseName,
                        c.Code AS CourseCode,
                        COALESCE(c.NumberOfPoints, c.NumberOfCreditHours) AS NumberOfPoints,
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
                        AssessMethods.Name AS AssessMethodsName,
                        CASE
                            WHEN StudentSemesterAssessMethods.Degree IS NOT NULL THEN CAST(StudentSemesterAssessMethods.Degree AS VARCHAR(50))
                            ELSE 'Waiting'
                        END AS Degree
                    FROM
                        StudentSemesters AS ss 
                    JOIN 
                        Students ON Students.Id = ss.StudentId
                    JOIN 
                        AspNetUsers AS ui ON ui.Id = Students.UserId
                    JOIN 
                        StudentSemesterCourses AS ssc ON ssc.StudentSemesterId = ss.Id
                    JOIN 
                        Courses AS c ON c.Id = ssc.CourseId
                    JOIN 
                        CourseAssessMethods AS cam ON cam.CourseId = c.Id
                    JOIN 
                        AssessMethods ON AssessMethods.Id = cam.AssessMethodId
                    JOIN 
                        StudentSemesterAssessMethods ON StudentSemesterAssessMethods.CourseAssessMethodId = cam.Id
                    WHERE 
                        (ss.AcademyYearId = @AcademyYearId AND 
                        ss.ScientificDegreeId = @ScientificDegreeId AND 
                        ssc.CourseId = @CourseId) AND
                        (ss.Id = StudentSemesterAssessMethods.StudentSemesterId);
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetAllStudentInCourseResult]");
        }
    }
}
