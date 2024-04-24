using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentCourseInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentCourseInfo]
                @CourseId INT
                AS
                BEGIN
                    SELECT
                        StudentSemesterCourses.Id AS StudentSemesterCourseId,
                        Courses.Code AS CourseCode,
                        Courses.Name AS CourseName,
                        Students.Code AS StudentCode,
                        AspNetUsers.NameEnglish AS StudentName,
                        StudentSemesterCourses.Notes
                    FROM
                        StudentSemesterCourses
                    JOIN
                        Courses ON Courses.Id = StudentSemesterCourses.CourseId
                    JOIN
                        StudentSemesters ON StudentSemesters.Id = StudentSemesterCourses.StudentSemesterId
                    JOIN
                        AcademyYears ON AcademyYears.Id = StudentSemesters.AcademyYearId
                    JOIN
                        Students ON Students.Id = StudentSemesters.StudentId
                    JOIN
                        AspNetUsers ON AspNetUsers.Id = Students.UserId
                    WHERE
                        StudentSemesterCourses.CourseId = @CourseId
                        AND AcademyYears.IsCurrent = 1;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
