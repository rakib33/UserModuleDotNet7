using MailKit.Net.Smtp;
using MimeKit;

using UserManagement.EmailService.Models;

namespace UserManagement.EmailService.Services
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
    
                using var client = new SmtpClient();
            //using (var client = new SmtpClient())
            //{
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
              //  client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                  //  client.Connect("smtp.server.com", 587, false);
                    client.Connect(_emailConfiguration.SmtpServer,_emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.UserName,_emailConfiguration.Password);
                   
                    client.Send(message);
       
              //  }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally {
                client.Disconnect(true);
                client.Dispose(); 
            }
        }
    }
}
