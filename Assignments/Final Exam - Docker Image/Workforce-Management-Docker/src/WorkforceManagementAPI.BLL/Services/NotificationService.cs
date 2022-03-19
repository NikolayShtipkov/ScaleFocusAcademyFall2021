using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models;

namespace WorkforceManagementAPI.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MailSettings _mailSettings;

        public NotificationService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task Send(List<User> receivers, string subject, string message)
        {
            var email = new MimeMessage();
            var receiversMail = new InternetAddressList();

            foreach(string mail in receivers.Select(r => r.Email))
            {
                receiversMail.Add(MailboxAddress.Parse(mail));
            }

            email.From.Add(MailboxAddress.Parse(_mailSettings.SenderEmail));
            email.To.AddRange(receiversMail);
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = message };

            var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors) => true;

            await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(new NetworkCredential(_mailSettings.SenderEmail, _mailSettings.Password));
            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            client.Dispose();
        }
    }
}
