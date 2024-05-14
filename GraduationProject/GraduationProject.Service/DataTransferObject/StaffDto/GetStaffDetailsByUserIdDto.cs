using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetStaffDetailsByUserIdDto
    {
        public string NameEnglish { get; set; }
        public string NameArabic { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public int StaffId { get; set; }
        public string StaffAddress { get; set; }
        [DataType(DataType.Date)]

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public string? PostalCode { get; set; }
        public string? ReleasePlace { get; set; }
        public string Religion { get; set; }
        public string FacultyName { get; set; }
        public string? PreQualification { get; set; }
        [DataType(DataType.Date)]

        public DateTime? QualificationYear { get; set; }
        public int? SeatNumber { get; set; }
        public decimal? Degree { get; set; }
        public List<GetPhoneSafftDto>? GetPhoneStaffDtos { get; set; } = new List<GetPhoneSafftDto>();
    }
    public class GetPhoneSafftDto
    {
        public string? StaffPhoneNumber { get; set; }
        public string? PhoneType { get; set; }
        public int? PhoneId { get; set; }

    }
}

