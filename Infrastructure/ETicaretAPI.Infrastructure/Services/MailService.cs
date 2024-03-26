using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {
            StringBuilder mail = new();
            mail.AppendLine($"Sayın {userName} Merhaba");
            mail.AppendLine("<br>");
            mail.AppendLine($"{orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir");
            mail.AppendLine("<br>");

            string subject = $"{orderCode} Siparişi Numaralı Siparişiniz Tamamlandı";

            await SendMailAsync(to, subject, mail.ToString());
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new()
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
            };
            foreach (var to in tos)
                mail.To.Add(to);

            mail.From = new(_configuration["Mail:Username"], "S Ticaret", System.Text.Encoding.UTF8);

            SmtpClient smtp = new()
            {
                Port = Convert.ToInt32(_configuration["Mail:Port"]),
                EnableSsl = true,
                Host = _configuration["Mail:Host"],
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"])
            };

            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Merhaba");
            mail.AppendLine("<br>");
            mail.AppendLine("Eğer yeni şifre talebinde bulundurysanız aşağıdaki linkten şifreyi yenileyebilirsiniz.");
            mail.AppendLine("<br>");
            string link = $"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}";
            mail.AppendLine($"<strong> <a target=\"_blank\" href=\"{link}\">Yeni şifre talebi için tıklayınız...</a> </strong>");
            mail.AppendLine("<br>");
            mail.AppendLine("<span style=\"font-size:12px;\">NOT : Eğer bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span>");
            mail.AppendLine("<br>");
            mail.AppendLine("Saygılarımızla...");
            mail.AppendLine("<br>");
            mail.AppendLine("<br>");
            mail.AppendLine("Selek Mini|E Ticaret");

            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
        }
    }
}