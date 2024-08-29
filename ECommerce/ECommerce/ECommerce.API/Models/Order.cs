using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommerce.API.Models
{
    public class Order
    {
         [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public Cart Cart { get; set; } = new Cart();
        public Payment Payment { get; set; } = new Payment();
        public string CreatedAt { get; set; } = string.Empty;
    }
}
