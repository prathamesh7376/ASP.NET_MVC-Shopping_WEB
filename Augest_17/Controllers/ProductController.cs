using Augest_17.Data;
using Augest_17.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Augest_17.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        public readonly ApplicationDbContext db;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int? page)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "User");
            }
            int pageSize = 5; // Number of products per page
         int pageNumber = page ?? 1; // Default to page 1

         var products = db.Products
                     .OrderBy(p => p.Name) // Optional: order for consistency
                     .ToPagedList(pageNumber, pageSize);

         return View(products);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Details(Guid id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("_ProductDetails", product);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Product product = await db.Products.FirstOrDefaultAsync(p=>p.Id ==id);
            if(product != null)
            {
                return View(product);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(); // More appropriate than NotFound for ID mismatch
            }

            if (!ModelState.IsValid)
            {
                return View(product); // Return the same view with validation errors
            }

            var existingProduct = await db.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(); // Product doesn't exist
            }

            // Optionally map updated fields to existingProduct to avoid overposting
            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
