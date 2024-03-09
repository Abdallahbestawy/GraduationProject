
using GraduationProject.Identity.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(500)]
        public string NameArabic { get; set; }
        [Required, MaxLength(500)]
        public string NameEnglish { get; set; }
        [Required, MaxLength(14)]
        [RegularExpression(@"^\d+$")]
        public string NationalID { get; set; }
        public UserType? UserType { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
