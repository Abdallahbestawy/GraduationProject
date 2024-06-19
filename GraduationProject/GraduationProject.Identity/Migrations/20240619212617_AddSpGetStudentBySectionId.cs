using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentBySectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentBySectionId]
                    @ScheduleId INT
                AS
                BEGIN
                    SELECT 
                        AspNetUsers.NameEnglish,
                        Students.Code 
                    FROM 
                        StudentSchedules
                    JOIN 
                        Students ON Students.Id = StudentSchedules.StudentId
                    JOIN 
                        AspNetUsers ON AspNetUsers.Id = Students.UserId
                    WHERE 
                        StudentSchedules.ScheduleId = @ScheduleId;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentBySectionId]");
        }
    }
}
