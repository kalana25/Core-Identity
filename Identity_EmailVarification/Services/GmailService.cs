using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Identity_EmailVarification.Services;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Identity_EmailVarification.Services
{
    public class GmailService:IGmailService
    {
        private readonly IOptions<Gmail> options;
        public GmailService(IOptions<Gmail> opts)
        {
            options = opts;
        }
        public async void SendAsync(string destinationEmail,string subject,string content)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(options.Value.SourceEmail);
                    message.To.Add(destinationEmail);
                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    message.Body = content;
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(options.Value.SourceEmail,options.Value.Password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
