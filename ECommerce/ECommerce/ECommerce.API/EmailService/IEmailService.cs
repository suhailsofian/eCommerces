
using ECommerce.API.EmailSettings;

namespace ECommerce.API
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
    }
}
