using MailKit.Net.Smtp;
using MimeKit;

using UserManagement.Service.Models;

namespace UserManagement.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration) =>_emailConfiguration= emailConfiguration;
     

        public void SendMail(Message message)
        {
           var emailMesssage = CreateEmailMessage(message);
            Send(emailMesssage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;        
         }

        private void Send(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.server.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("username", "password");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
