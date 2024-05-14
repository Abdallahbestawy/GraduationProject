using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models
{
    public class GetStudentInfoByStudentIdModel
    {
        public string NameEnglish { get; set; }
        public string NameArabic { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public int StudentId { get; set; }
        public string? StudentsStreet { get; set; }
        public int? StudentsCountrysId { get; set; }
        public int? StudentsGovernoratesId { get; set; }
        public int? StudentsCitysId { get; set; }
        [DataType(DataType.Date)]

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public string? PostalCode { get; set; }
        public string? ReleasePlace { get; set; }
        public Religion Religion { get; set; }
        public int FacultyId { get; set; }
        public string ParentName { get; set; }
        public string Code { get; set; }

        public string ParentJob { get; set; }
        public string? PostalCodeOfParent { get; set; }
        public string? ParentStreet { get; set; }
        public int? ParentCountrysId { get; set; }
        public int? ParentGovernoratesId { get; set; }
        public int? ParentCitysId { get; set; }
        public string? PreQualification { get; set; }
        [DataType(DataType.Date)]

        public DateTime? QualificationYear { get; set; }
        public int? SeatNumber { get; set; }
        public decimal? Degree { get; set; }
        public string? StudentPhoneNumber { get; set; }
        public PhoneType? PhoneType { get; set; }
        public int? PhoneId { get; set; }
    }
}
