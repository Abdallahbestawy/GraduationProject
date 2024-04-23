using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class AddSpGetStudentDetailsByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpGetStudentDetailsByUserId]
                    @UserId NVARCHAR(450)
                AS
                BEGIN
                    SELECT 
                        AspNetUsers.NameEnglish,
                        AspNetUsers.NameArabic,
                        AspNetUsers.NationalID,
                        AspNetUsers.Email,
                        Students.Id as 'StudentId',
                        CONCAT(Countrys.Name, ',', Governorates.Name, ',', Citys.Name, ',', Students.Street) as 'StudentAddress',
                        Students.DateOfBirth as 'DateOfBirth', 
                        Students.Gender,
                        Students.Code,
                        Students.Nationality,
                        Students.PlaceOfBirth,
                        Students.PostalCode,
                        Students.ReleasePlace,
                        Students.Religion,
                        FamilyDatas.ParentName,
                        FamilyDatas.Job as 'ParentJob',
                        FamilyDatas.PostalCode as 'PostalCodeOfParent',
                        CONCAT(Countrys.Name, ',', Governorates.Name, ',', Citys.Name, ',',FamilyDatas.Street) as 'ParentAddress',
                        QualificationDatas.PreQualification,
                        QualificationDatas.QualificationYear AS 'QualificationYear',
                        QualificationDatas.SeatNumber,
                        QualificationDatas.Degree,
                        Phones.PhoneNumber as 'StudentPhoneNumber',
                        Phones.Type as 'PhoneType',
                        Phones.Id as PhoneId
                    FROM
                        AspNetUsers
                    JOIN
                        Students ON AspNetUsers.Id = Students.UserId
                    JOIN
                        Citys ON Citys.Id = Students.CityId
                    JOIN
                        Countrys ON Countrys.Id = Students.CountryId
                    JOIN
                        Governorates ON Governorates.Id = Students.GovernorateId
                    LEFT JOIN
                        FamilyDatas ON FamilyDatas.StudentId = Students.Id
                    JOIN
                        QualificationDatas ON QualificationDatas.StudentId = Students.Id
                    LEFT JOIN
                        Phones ON Phones.StudentId = Students.Id
                    WHERE
                        AspNetUsers.Id = @UserId;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[SpGetStudentDetailsByUserId]");
        }
    }
}
