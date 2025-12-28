using Inventory_Management_.NET.Dtos;
using System.Net;
using System.Net.Mail;

namespace Inventory_Management_.NET.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(EmailDto dto)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var fromAddress = new MailAddress(emailSettings["SenderEmail"]!, emailSettings["SenderName"]);
            var toAddress = new MailAddress(dto.To);

            using (var smtp = new SmtpClient())
            {
                smtp.Host = emailSettings["SmtpServer"]!;
                smtp.Port = int.Parse(emailSettings["Port"]!);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(emailSettings["Username"]!, emailSettings["Password"]!);

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = dto.Subject,
                    Body = dto.Body,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
    }

}
