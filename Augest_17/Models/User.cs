using System.ComponentModel.DataAnnotations;

namespace Augest_17.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "User";

        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ✅ Navigation property
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
