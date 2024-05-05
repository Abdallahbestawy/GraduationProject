using GraduationProject.Data.Enum;
using GraduationProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.PhoneDto
{
    public class PhoneNumberDto
    {
        [IgnoreFromExcelFIle]
        public int? Id { get; set; }
        [Required, MaxLength(11)]
        [RegularExpression(@"^\d+$")]
        public string PhoneNumber { get; set; }

        public PhoneType Type { get; set; }
    }
}
