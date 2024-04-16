using Movies_Point.IRepository;
using System.Net;
using System.Net.Mail;
namespace Movies_Point.ViewModels
{
    public class EmailSender : IEmailSenderRepository
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = _configuration["EmailSettings:Email"];
            var pw = _configuration["EmailSettings:Password"];
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
    }
}
