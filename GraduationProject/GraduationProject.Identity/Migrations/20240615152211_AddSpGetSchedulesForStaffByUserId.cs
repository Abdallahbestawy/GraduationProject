using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetSchedulesForStaffByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetSchedulesForStaffByUserId]
                    @UserId nvarchar(450)
                AS
                BEGIN
                    SELECT 
                        Schedules.Id as SchedulesId,
                        ScheduleDay,
                        ScheduleType,
                        TimeStart,
                        EndStart,
                        Facultys.Name AS FacultysName,
                        AspNetUsers.NameEnglish AS NameEnglish,
                        CONCAT(YEAR(AcademyYears.Start), '-', YEAR(AcademyYears.[End])) AS AcademyYear,
                        SchedulePlaces.Name AS SchedulePlacesName,
                        ScientificDegrees.Name AS ScientificDegreesName
                    FROM
                        Schedules
                    JOIN Facultys
                        ON Facultys.Id = Schedules.FacultyId
                    JOIN Staffs 
                        ON Staffs.Id = Schedules.StaffId
                    JOIN AspNetUsers 
                        ON AspNetUsers.Id = Staffs.UserId
                    JOIN SchedulePlaces
                        ON SchedulePlaces.Id = Schedules.SchedulePlaceId
                    JOIN ScientificDegrees
                        ON ScientificDegrees.Id = Schedules.ScientificDegreeId
                    JOIN AcademyYears
                        ON AcademyYears.Id = Schedules.AcademyYearId
                    WHERE 
                        AspNetUsers.Id = @UserId 
                        AND AcademyYears.IsCurrent = 1;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetSchedulesForStaffByUserId]");
        }

    }
}
