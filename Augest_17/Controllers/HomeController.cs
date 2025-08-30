using System.Diagnostics;
using Augest_17.Data;
using Augest_17.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Augest_17.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            db = dbContext;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            List<Product> products = db.Products.ToList();

            if (!string.IsNullOrEmpty(username))
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    // ✅ fetch liked product ids
                    var userLikes = db.Likes
                        .Where(l => l.UserId == user.Id)
                        .Select(l => l.ProductId)
                        .ToList();

                    // ✅ fetch cart product ids
                    var userCart = db.CartItems
                        .Where(c => c.UserId == user.Id)
                        .Select(c => c.ProductId)
                        .ToList();

                    ViewBag.UserLikes = userLikes;
                    ViewBag.UserCart = userCart;
                }
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
