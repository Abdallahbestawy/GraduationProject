using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Identity.Models
{
    public class LoginUserModel
    {
        [Required, MaxLength(14)]
        [RegularExpression(@"^\d+$")]
        public string NationalID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
