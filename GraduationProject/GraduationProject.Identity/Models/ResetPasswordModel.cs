using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Identity.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Password and ConfirmPassword Not matched")]
        //public string ConfirmPassword { get; set; }

        [Required]
        public string token { get; set; }
    }
}
