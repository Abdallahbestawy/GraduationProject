using GraduationProject.Mails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Mails.IService
{
    public interface IMailService
    {
        Task SendExceptionEmail(ExceptionEmailModel model);
    }
}
