using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Augest_17.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int UserId { get; set; }   // Changed to int
        public User User { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAtAdd { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
