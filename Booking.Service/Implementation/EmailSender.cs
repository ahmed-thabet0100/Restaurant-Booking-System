using Restaurant.Service.Abstracts;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Restaurant.Service.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string ToEmail, string Subject, string Message)
        {
            var smtpUser = _config["Email:SmtpUser"]; // البريد بتاعك
            var smtpPass = _config["Email:SmtpPass"]; // App password لو Gmail
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;

            var mail = new MailMessage();
            mail.From = new MailAddress(smtpUser);
            mail.To.Add(ToEmail);
            mail.Subject = Subject;
            mail.Body = Message;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);

        }
        // 🔥 دي اللي Hangfire هيستخدمها
        public void SendEmail(string ToEmail, string Subject, string Message)
        {
            SendEmailAsync(ToEmail, Subject, Message).GetAwaiter().GetResult();
        }
    }
}
