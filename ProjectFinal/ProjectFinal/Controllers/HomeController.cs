using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectFinal.Models;
using System.Diagnostics;

namespace ProjectFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FoodShopContext _foodShopContext;

        public HomeController(ILogger<HomeController> logger, FoodShopContext foodShopContext)
        {
            _logger = logger;
            _foodShopContext = foodShopContext;
        }

        public async Task<IActionResult> Index()
        {
            var customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                var existingCustomer = JsonConvert.DeserializeObject<Customer>(customerJson);
                if (existingCustomer.Role == 1)
                {
                    return RedirectToAction("ManageProducts", "Admin");
                }
            }

            var productsByCategory = await _foodShopContext.Categories
                .Include(c => c.Products)
                .Select(c => new
                {
                    CategoryName = c.CategoryName,
                    Products = c.Products.OrderBy(p => p.ProductId).Take(3),
                })
            .ToListAsync();

            var model = productsByCategory.Select(c => new
            {
                c.CategoryName,
                Products = c.Products.ToList()
            }).ToList();

            var listRemainingProducts = _foodShopContext.Products.Skip(2)
                .Where(p => p.Category.CategoryName == "Sushi")
                .ToList();
            ViewBag.Products = listRemainingProducts;

            var categories = await _foodShopContext.Categories.ToListAsync();

            ViewBag.Categories = categories;

            return View(model);
        }

        public async Task<IActionResult> ProductDetail(int? id)
        {
            var customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                var existingCustomer = JsonConvert.DeserializeObject<Customer>(customerJson);
                if (existingCustomer.Role == 1)
                {
                    return RedirectToAction("AccessDenied", "Authentication");
                }
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = await _foodShopContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var randomRelatedProducts = await _foodShopContext.Products
                .OrderBy(p => Guid.NewGuid()) 
                .Take(3) 
                .ToListAsync();

            ViewBag.RandomRelatedProducts = randomRelatedProducts;

            return View(product);
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult OurHistory()
        {
            return View();
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
