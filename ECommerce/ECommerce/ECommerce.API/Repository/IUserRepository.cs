using ECommerce.API.Models;

namespace ECommerce.API.Repository
{
    public interface IUserRepository
    {
        Task<bool> InsertUserAsync(UserDto user);
        string IsUserPresent(string email, string password);
        User GetUser(int id);
        }
}
