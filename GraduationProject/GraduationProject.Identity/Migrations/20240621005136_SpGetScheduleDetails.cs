using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class SpGetScheduleDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetScheduleDetails]
                    @FacultyId INT,
                    @ScientificDegreeId INT
                AS
                BEGIN
                    SELECT 
                        Schedules.Id As SchedulesId
                        Schedules.ScheduleType,
                        Schedules.ScheduleDay,
                        CAST(Schedules.TimeStart AS VARCHAR(5)) + ' - ' + CAST(Schedules.EndStart AS VARCHAR(5)) AS Timing,
                        Schedules.Capacity,
                        Facultys.Name AS FacultyName,
                        AspNetUsers.NameEnglish As StaffName,
                        SchedulePlaces.Name AS SchedulePlacesName,
                        Courses.Name AS CourseName,
                        Courses.Code AS CourseCode,
                        CONCAT(YEAR(AcademyYears.Start), '-', YEAR(AcademyYears.[End])) AS AcademyYear,
                        ScientificDegrees.Name AS ScientificDegreesName,
                        Parent.Name AS BandName
                    FROM 
                        Schedules
                    JOIN 
                        Facultys ON Facultys.Id = Schedules.FacultyId
                    JOIN 
                        Staffs ON Staffs.Id = Schedules.StaffId
                    JOIN 
                        AspNetUsers ON AspNetUsers.Id = Staffs.UserId
                    JOIN 
                        SchedulePlaces ON SchedulePlaces.Id = Schedules.SchedulePlaceId
                    JOIN 
                        Courses ON Courses.Id = Schedules.CourseId
                    JOIN 
                        ScientificDegrees ON ScientificDegrees.Id = Schedules.ScientificDegreeId
                    JOIN 
                        ScientificDegrees AS Parent ON ScientificDegrees.ParentId = Parent.Id
                    JOIN 
                        AcademyYears ON AcademyYears.Id = Schedules.AcademyYearId
                    WHERE 
                        Schedules.FacultyId = @FacultyId
                        AND Schedules.ScientificDegreeId = @ScientificDegreeId
                        AND AcademyYears.IsCurrent = 1;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetScheduleDetails]");
        }
    }
}
