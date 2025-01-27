using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Utils
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration _configuration)
        {
            SendGridSecret = _configuration.GetValue<string>("SendGrid:SecretKey");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var cliente = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("stevenfranco0296@gmail.com");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return cliente.SendEmailAsync(msg);
        }
    }
}
