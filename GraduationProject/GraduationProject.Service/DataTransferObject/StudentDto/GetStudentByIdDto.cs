using GraduationProject.Service.DataTransferObject.PhoneDto;

namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class GetStudentByIdDto
    {
        public int Id { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public string PlaceOfBirth { get; set; }

        public string GenderName { get; set; }

        public string NationalityName { get; set; }

        public string ReligionName { get; set; }

        public string? ReleasePlace { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string CountryName { get; set; }

        public string GovernorateName { get; set; }

        public string CityName { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }
        public string? PreQualification { get; set; }

        public int? SeatNumber { get; set; }

        public DateTime? QualificationYear { get; set; }

        public decimal? Degree { get; set; }
        public string ParentName { get; set; }

        public string ParentJob { get; set; }

        public string ParentCountryName { get; set; }

        public string ParentGovernorateName { get; set; }


        public string ParentCityName { get; set; }


        public string? ParentStreet { get; set; }
        public List<PhoneNumberDto>? PhoneNumbers { get; set; }
    }
}
