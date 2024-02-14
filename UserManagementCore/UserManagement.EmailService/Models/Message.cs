using MimeKit;

namespace UserManagement.EmailService.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public String Subject { get; set; }
        public String Content { get; set; }
        public Message(IEnumerable<string> to,string subject, string content) 
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x=>new MailboxAddress("email",x)));
            Subject = subject;
            Content = content;

        }
    }
}
