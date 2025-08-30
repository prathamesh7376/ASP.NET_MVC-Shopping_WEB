using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Augest_17.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Precision(18, 2)]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100000")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Image URL must be a valid URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int Stock { get; set; }

        // ✅ Navigation property
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
