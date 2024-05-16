using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetAllStaffs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetAllStaffs]
                    @UserType smallint,
                    @FacultyId INT
                AS
                BEGIN
                    SELECT
                        Staffs.Id,
                        Staffs.UserId,
                        AspNetUsers.NameArabic,
                        AspNetUsers.NameEnglish,
                        AspNetUsers.Email,
                        Staffs.Gender,
                        Staffs.Nationality,
                        Staffs.Religion
                    FROM
                        Staffs
                    JOIN
                        AspNetUsers ON Staffs.UserId = AspNetUsers.Id AND AspNetUsers.UserType = @UserType
                    WHERE
                        Staffs.FacultyId = @FacultyId;
                END;
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetAllStaffs]");
        }
    }
}
