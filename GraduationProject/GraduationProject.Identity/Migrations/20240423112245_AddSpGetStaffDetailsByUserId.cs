using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStaffDetailsByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStaffDetailsByUserId]
                    @UserId NVARCHAR(450)
                AS
                BEGIN
                    SELECT 
                        AspNetUsers.NameEnglish,
                        AspNetUsers.NameArabic,
                        AspNetUsers.NationalID,
                        AspNetUsers.Email,
                        Staffs.Id,
                        CONCAT(Countrys.Name, ',', Governorates.Name, ',', Citys.Name, ',', Staffs.Street) as 'StaffAddress',
                        Staffs.DateOfBirth,
                        Staffs.Gender,
                        Staffs.Nationality,
                        Staffs.PlaceOfBirth,
                        Staffs.PostalCode,
                        Staffs.ReleasePlace,
                        Staffs.Religion,
                        Facultys.Name As FacultysName, 
                        QualificationDatas.PreQualification,
                        QualificationDatas.QualificationYear,
                        QualificationDatas.SeatNumber,
                        QualificationDatas.Degree,
                        Phones.PhoneNumber as 'StaffPhoneNumber',
                        Phones.Type as 'PhoneType',
                        Phones.Id as PhoneId
                    FROM
                        AspNetUsers
                    JOIN
                        Staffs ON AspNetUsers.Id = Staffs.UserId
                    JOIN 
                        Facultys ON Facultys.Id = Staffs.FacultyId
                    JOIN
                        Citys ON Citys.Id = Staffs.CityId
                    JOIN
                        Countrys ON Countrys.Id = Staffs.CountryId
                    JOIN
                        Governorates ON Governorates.Id = Staffs.GovernorateId
                    JOIN
                        QualificationDatas ON QualificationDatas.StaffId = Staffs.Id
                    LEFT JOIN
                        Phones ON Phones.StaffId = Staffs.Id
                    WHERE
                        AspNetUsers.Id = @UserId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStaffDetailsByUserId]");
        }
    }
}
