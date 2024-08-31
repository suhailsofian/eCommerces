using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ECommerce.API.Models
{
    public class UserLoginDto
    {
               public string Email { get; set; } = string.Empty;
       
        public string Password { get; set; } = string.Empty;
    }
}
