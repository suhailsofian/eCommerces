using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ECommerce.API.Models
{
    public class Order
    {
         [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // [JsonIgnore]
        public User User { get; set; } = new User();
        //  public int UserId { get; set; }
        //   [JsonIgnore]
        public Cart Cart { get; set; } = new Cart();
        //  public int CartId { get; set; }
        //  [JsonIgnore]
        public Payment Payment { get; set; } = new Payment();
        // public int PaymentId { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
