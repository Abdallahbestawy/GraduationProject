using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Mails.Models
{
    public class ExceptionEmailModel
    {
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime Time { get; set; }
        public string? StackTrace { get; set; }
    }
}
