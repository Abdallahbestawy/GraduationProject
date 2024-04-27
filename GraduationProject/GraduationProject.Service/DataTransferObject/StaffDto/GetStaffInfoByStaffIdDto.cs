using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetStaffInfoByStaffIdDto
    {
        public string NameEnglish { get; set; }
        public string NameArabic { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public int StaffId { get; set; }
        public string? StaffStreet { get; set; }
        public int? StaffCountryId { get; set; }
        public int? StaffGovernorateId { get; set; }
        public int? StaffCityId { get; set; }
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
        public List<GetPhoneTypeDto>? GetPhoneStaffDtos { get; set; } = new List<GetPhoneTypeDto>();
    }
}
