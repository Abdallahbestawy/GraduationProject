using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.PhoneDto
{
    public class GetPhoneTypeDto
    {
        public int? PhoneId { get; set; }
        public string? PhoneNumber { get; set; }
        public PhoneType? PhoneType { get; set; }
    }
}
