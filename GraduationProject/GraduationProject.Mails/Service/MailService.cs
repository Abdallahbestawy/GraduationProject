using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Mails.Templates;
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
        public async Task<string> SendExceptionEmail(ExceptionEmailModel model)
        {
            try
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

                var mailText = ExceptionsTemplate.Value;

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
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<bool> SendResetPasswordEmail(ResetPasswordEmailModel model)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = "Reset Password"
            };

            email.To.Add(MailboxAddress.Parse(model.Email));

            var builder = new BodyBuilder();

            var mailText = ResetPasswordTemplate.Value;

            mailText = mailText.Replace("[UserName]", model.UserName).Replace("[ResetURL]", model.ResetURL);

            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));


            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            var result = await smtp.SendAsync(email);

            smtp.Disconnect(true);

            return true;
        }
    }
}
