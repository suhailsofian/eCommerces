using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommerce.API.Models
{
    public class CartItem
    {
         [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Cart? Cart { get; set; }
      public int CartId { get; set; }
        public Product? Product { get; set; }
        //   public int ProductId { get; set; }
    }
}
