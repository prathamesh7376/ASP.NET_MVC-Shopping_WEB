using Augest_17.Data;
using Augest_17.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Augest_17.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;

        public CartController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Guid productId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var product = await db.Products.FindAsync(productId);
            if (product == null) return NotFound();

            // check if already in cart
            var existingCartItem = await db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity = 1;
                await db.SaveChangesAsync();
                return Json(new { added = true, quantity = existingCartItem.Quantity });
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Quantity = 1,
                    PriceAtAdd = product.Price,
                    AddedAt = DateTime.UtcNow
                };

                db.CartItems.Add(cartItem);
                await db.SaveChangesAsync();
                return Json(new { added = true, quantity = 1 });
            }
        }
    }
}
