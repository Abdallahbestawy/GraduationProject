using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models
{
    public class GetStaffInfoByStaffIdModel
    {
        public string NameEnglish { get; set; }
        public string NameArabic { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public int StaffId { get; set; }
        public string? StaffStreet { get; set; }
        public int? StaffCountrysId { get; set; }
        public int? StaffGovernoratesId { get; set; }
        public int? StaffCitysId { get; set; }
        [DataType(DataType.Date)]

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public string? PostalCode { get; set; }
        public string? ReleasePlace { get; set; }
        public Religion Religion { get; set; }
        public string? PreQualification { get; set; }
        [DataType(DataType.Date)]
        public DateTime? QualificationYear { get; set; }
        public int? SeatNumber { get; set; }
        public decimal? Degree { get; set; }
        public string? StaffPhoneNumber { get; set; }
        public PhoneType? PhoneType { get; set; }
        public int? PhoneId { get; set; }
    }
}
