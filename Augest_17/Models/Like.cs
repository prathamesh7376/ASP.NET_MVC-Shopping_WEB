using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Augest_17.Models
{
    public class Like
    {
        public int Id { get; set; }   // Primary key

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }   // ✅ make nullable

        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }   // ✅ make nullable

        [Required]
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Precision(18, 2)]
        public decimal PriceAtLike { get; set; }
    }
}
