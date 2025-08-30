using Augest_17.Data;
using Augest_17.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Augest_17.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext db;
        public LikeController(ApplicationDbContext context)
        {
            db = context;
        }
        // Replace all instances of '_context' with 'db' to match the constructor-initialized field.

        [HttpPost]
        public async Task<IActionResult> ToggleLike(Guid productId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var product = await db.Products.FindAsync(productId);
            if (product == null) return NotFound();

            // check if already liked
            var existingLike = await db.Likes
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.ProductId == productId);

            if (existingLike != null)
            {
                // unlike
                db.Likes.Remove(existingLike);
                await db.SaveChangesAsync();
                return Json(new { liked = false });
            }
            else
            {
                // like
                var like = new Like
                {
                    UserId = user.Id,
                    ProductId = productId,
                    PriceAtLike = product.Price,
                    LikedAt = DateTime.UtcNow
                };
                db.Likes.Add(like);
                await db.SaveChangesAsync();
                return Json(new { liked = true });
            }
        }


    }
}
