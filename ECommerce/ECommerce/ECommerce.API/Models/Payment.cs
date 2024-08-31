using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ECommerce.API.Models
{
    public class Payment
    {
         [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // [JsonIgnore]
        public PaymentMethod? PaymentMethod { get; set; }
        //  public int PaymentMethodId { get; set;}
        //  [JsonIgnore]
        public User? User { get; set; }
        // public int UserId { get; set;}
        public int TotalAmount { get; set; }
        public int ShipingCharges { get; set; }
        public int AmountReduced { get; set; }
        public int AmountPaid { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}
