using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus]
                    @CourseId INT,
                    @IsControlStatus BIT
                AS
                BEGIN
                    SELECT
                        u.NameEnglish AS 'StudentName',
                        s.Code AS 'StudentCode',
                        c.Name AS 'CourseName',
                        c.Code AS 'CourseCode',
                        c.Id AS 'CourseId',
                        ssm.Degree,
                        CONCAT(am.Name, '(', am.MaxDegree,' MaxDegree', ')') AS 'AssessmentMethodName',
                        am.Id AS 'AssessmentMethodId',
                        ssm.Id AS 'StudentSemesterAssessMethodsId',
                        am.IsControlStatus
                    FROM
                        AspNetUsers u
                    JOIN
                        Students s ON u.Id = s.UserId
                    JOIN
                        StudentSemesters ss ON s.Id = ss.StudentId
                    JOIN
                        AcademyYears ay ON ss.AcademyYearId = ay.Id 
                    JOIN
                        StudentSemesterAssessMethods ssm ON ss.Id = ssm.StudentSemesterId
                    JOIN
                        CourseAssessMethods cam ON ssm.CourseAssessMethodId = cam.Id
                    JOIN
                        AssessMethods am ON am.Id = cam.AssessMethodId
                    JOIN
                        Courses c ON c.Id = cam.CourseId
                    JOIN
                        StudentSemesterCourses ssc ON c.Id = ssc.CourseId
                    WHERE
                        c.Id = @CourseId  
                        AND ay.IsCurrent = 1 
                        AND am.IsControlStatus = @IsControlStatus
                    GROUP BY
                        u.NameEnglish,
                        s.Code,
                        c.Name,
                        c.Code,
                        c.Id,
                        ssm.Degree,
                        am.Name,
                        am.MaxDegree,
                        ssm.Id,
                        am.IsControlStatus,
                        am.Id
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus]");
        }
    }
}
