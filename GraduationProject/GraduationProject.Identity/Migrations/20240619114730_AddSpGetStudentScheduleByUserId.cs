using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentScheduleByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentScheduleByUserId]
                    @UserId nvarchar(450)
                AS
                BEGIN
                    SELECT 
                        Schedules.ScheduleDay,
                        Schedules.ScheduleType,
                        SchedulePlaces.Name AS SchedulePlacesName,
                        CAST(Schedules.TimeStart AS VARCHAR(5)) + ' - ' + CAST(Schedules.EndStart AS VARCHAR(5)) AS Timing,
                        Courses.Name AS CoursesName,
                        Courses.Code AS CoursesCode,
                        AspNetUsers.NameEnglish
                    FROM 
                        Students
                    JOIN 
                        StudentSemesters ON StudentSemesters.StudentId = Students.Id
                    JOIN 
                        StudentSchedules ON StudentSchedules.StudentId = Students.Id
                    JOIN 
                        Schedules ON Schedules.Id = StudentSchedules.ScheduleId
                    JOIN 
                        SchedulePlaces ON SchedulePlaces.Id = Schedules.SchedulePlaceId
                    JOIN 
                        Courses ON Courses.Id = Schedules.CourseId
                    JOIN 
                        Staffs ON Staffs.Id = Schedules.StaffId
                    JOIN 
                        AspNetUsers ON AspNetUsers.Id = Staffs.UserId
                    JOIN 
                        AcademyYears ON AcademyYears.Id = Schedules.AcademyYearId
                    WHERE 
                        StudentSemesters.TotalCourses IS NULL 
                        AND StudentSemesters.Total IS NULL
                        AND AcademyYears.IsCurrent = 1 
                        AND Students.UserId = @UserId 
                        AND Schedules.ScientificDegreeId = StudentSemesters.ScientificDegreeId;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentScheduleByUserId]");
        }
    }
}
