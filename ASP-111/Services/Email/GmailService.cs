using System.Net;
using System.Net.Mail;

namespace ASP_111.Services.Email
{
    public class GmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public GmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string email, string message, string subject)
        {
            var gmailSection = _configuration.GetSection("Smtp").GetSection("Gmail");
            String? host = gmailSection?.GetValue<String>("Host");
            int? port = gmailSection?.GetValue<int>("Port");
            String? box = gmailSection?.GetValue<String>("Email");
            String? password = gmailSection?.GetValue<String>("Password");
            bool? ssl = gmailSection?.GetValue<bool>("Ssl");
            if (host == null || port == null || box == null || password == null || ssl == null)
            {
                throw new ApplicationException("Configuration for SMTP::Gmail not full");
            }
            SmtpClient smtpClient = new(host)
            {
                Port = port.Value,
                EnableSsl = ssl.Value,
                Credentials = new NetworkCredential(box, password)
            };
            MailMessage mailMessage = new()
            {
                From = new MailAddress(box),
                Body = message,
                IsBodyHtml = true,
                Subject = subject
            };
            mailMessage.To.Add(new MailAddress(email));
            smtpClient.Send(mailMessage);
        }
    }
}