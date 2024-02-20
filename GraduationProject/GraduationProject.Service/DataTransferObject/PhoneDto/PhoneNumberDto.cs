using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.PhoneDto
{
    public class PhoneNumberDto
    {
        public string PhoneNumber { get; set; }

        public PhoneType Type { get; set; }
    }
}
