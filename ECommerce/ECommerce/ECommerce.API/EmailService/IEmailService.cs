
using ECommerce.API.Models.EmailSettings;

namespace ECommerce.API
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
    }
}
