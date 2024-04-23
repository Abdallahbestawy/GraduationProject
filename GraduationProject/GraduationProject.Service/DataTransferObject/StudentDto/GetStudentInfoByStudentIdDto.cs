using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class GetStudentInfoByStudentIdDto
    {
        public string NameEnglish { get; set; }
        public string NameArabic { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public int StudentId { get; set; }
        public string? StudentStreet { get; set; }
        public int? StudentCountrysId { get; set; }
        public int? StudentGovernoratesId { get; set; }
        public int? StudentCitysId { get; set; }
        [DataType(DataType.Date)]

        public DateTime? DateOfBirth { get; set; }
        public string StudentCode { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public string? PostalCode { get; set; }
        public string? ReleasePlace { get; set; }
        public string Religion { get; set; }
        public string ParentName { get; set; }

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
        public List<GetPhoneStudentDto>? GetPhoneStudentDtos { get; set; } = new List<GetPhoneStudentDto>();
    }

}
