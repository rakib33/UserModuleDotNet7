using UserManagement.EmailService.Models;

namespace UserManagement.EmailService.Services
{
    public interface IEmailService
    {
        void SendMail(Message message);
    }
}
