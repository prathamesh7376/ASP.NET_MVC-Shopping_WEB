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
        public async Task<IActionResult> ToggleLike([FromBody] Guid productId)
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




        public async Task<IActionResult> MyLiked()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Unauthorized();

            // get all liked product IDs
            var likedProductIds = await db.Likes
                .Where(l => l.UserId == userId.Value)
                .Select(l => l.ProductId)
                .ToListAsync();

            // fetch products
            var likedProducts = await db.Products
                .Where(p => likedProductIds.Contains(p.Id))
                .ToListAsync();

            // set ViewBag for buttons
            ViewBag.UserLikes = likedProductIds;

            var cartProductIds = await db.CartItems
                .Where(c => c.UserId == userId.Value)
                .Select(c => c.ProductId)
                .ToListAsync();
            ViewBag.UserCart = cartProductIds;

            return View(likedProducts);
        }
    }
}
