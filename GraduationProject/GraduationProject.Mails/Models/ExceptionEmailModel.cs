using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Mails.Models
{
    public class ExceptionEmailModel
    {
        public List<string>? Emails { get; set; } = new List<string>
        {
            "mohamed.abdalla.f185@gmail.com",
            "abdallahbesstawy2000@gmail.com",
            "dev.mohamedsaadphp@gmail.com",
            "mohamed.drive185@gmail.com"
        };
        public string ErrorMessage { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime Time { get; set; }
        public string? StackTrace { get; set; }
    }
}
