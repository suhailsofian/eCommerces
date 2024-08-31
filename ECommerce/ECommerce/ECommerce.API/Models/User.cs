using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ECommerce.API.Models
{
    public class User
    {
         [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string ModifiedAt { get; set; } = string.Empty;
        [JsonIgnore]
          public ICollection<Order>? Orders { get; set; }
    }
}
