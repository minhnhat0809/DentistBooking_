using Microsoft.Extensions.Configuration;
using Service.Lib;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _config.GetSection("Smtp");
            var mail = smtpSettings["From"];
            var password = smtpSettings["Password"];
            var client = new SmtpClient()
            {
                Host = smtpSettings["Server"],
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                Credentials = new NetworkCredential(mail, password),
                UseDefaultCredentials = bool.Parse(smtpSettings["DefaultCredentials"]),
                Port = int.Parse(smtpSettings["Port"])
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true, 
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}
