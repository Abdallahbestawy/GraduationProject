using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Mails.Models
{
    public class ResetPasswordEmailModel
    {
        public string UserName { get; set; }
        public string ResetURL { get; set; }
        public string Email { get; set; }
    }
}
