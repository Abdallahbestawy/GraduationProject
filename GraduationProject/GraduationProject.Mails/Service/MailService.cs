using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GraduationProject.Mails.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendExceptionEmail(ExceptionEmailModel model)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = "Exception Notifications"
            };

            foreach (var emailTo in model.Emails)
            {
                email.To.Add(MailboxAddress.Parse(emailTo));
            }

            var builder = new BodyBuilder();

            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var filePath = Path.Combine(projectDirectory, "GraduationProject.Mails", "Templates", "exceptionsTemplate.html");

            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[className]", model.ClassName).Replace("[methodName]", model.MethodName)
                .Replace("[errorMessage]", model.ErrorMessage).Replace("[time]", model.Time.ToString()).Replace("[stackTrace]", model.StackTrace);

            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));


            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
