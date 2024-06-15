using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetCurrentStaffByCourseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetCurrentStaffByCourseId]
                    @CourseId INT,
                    @Type INT
                AS
                BEGIN
                    SELECT
                        Staffs.Id AS StaffsId,
                        AspNetUsers.NameEnglish
                    FROM StaffSemesters
                    JOIN Staffs ON Staffs.Id = StaffSemesters.StaffId
                    JOIN AspNetUsers ON AspNetUsers.Id = Staffs.UserId
                    JOIN AcademyYears ON AcademyYears.Id = StaffSemesters.AcademyYearId
                    WHERE StaffSemesters.CourseId = @CourseId
                      AND AcademyYears.IsCurrent = 1
                      AND StaffSemesters.Type = @Type;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetCurrentStaffByCourseId]");
        }

    }
}
