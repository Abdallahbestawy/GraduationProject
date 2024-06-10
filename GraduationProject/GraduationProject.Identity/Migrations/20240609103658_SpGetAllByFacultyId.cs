using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class SpGetAllByFacultyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetAllByFacultyId]
                    @FacultyId INT
                AS
                BEGIN
                    SELECT
                        Staffs.UserId,
                        AspNetUsers.NameEnglish
                    FROM
                        Staffs
                    JOIN
                        AspNetUsers ON Staffs.UserId = AspNetUsers.Id AND AspNetUsers.UserType != 1
                    WHERE 
                        Staffs.FacultyId = @FacultyId
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetAllByFacultyId]");
        }

    }
}
