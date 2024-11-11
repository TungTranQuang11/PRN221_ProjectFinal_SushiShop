using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectFinal.Models;
using Microsoft.AspNetCore.SignalR;
using ProjectFinal.Hubs;
using ProjectFinal.Services;

namespace ProjectFinal.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public ProductController(IProductService productService, IHubContext<SignalrServer> signalRHub)
        {
            _productService = productService;
            _signalRHub = signalRHub;
        }

        private IActionResult? CheckAdminAccess()
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
            return null;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, string? sortOrder = null, string? searchQuery = null, int? categoryId = null)
        {
            var accessCheck = CheckAdminAccess();
            if (accessCheck != null) return accessCheck;

            int pageSize = 3;
            var products = await _productService.GetProductsAsync(pageIndex, pageSize, sortOrder, searchQuery, categoryId);
            var categories = await _productService.GetCategoriesAsync();

            ViewBag.Categories = categories;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = searchQuery;
            ViewBag.CurrentCategory = categoryId ?? null;

            return View(products);
        }

        public IActionResult Cart()
        {
            var accessCheck = CheckAdminAccess();
            if (accessCheck != null) return accessCheck;

            var cartItems = GetCartItems();
            ViewBag.CartItems = cartItems;
            ViewBag.TotalItems = cartItems.Count;
            return View(cartItems);
        }

        private List<CartItem> GetCartItems()
        {
            var cookie = Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cookie))
            {
                return new List<CartItem>();
            }

            return JsonConvert.DeserializeObject<List<CartItem>>(cookie);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(c => c.ProductID == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    ProductID = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    ProductImage = product.Image
                });
            }

            SaveCartItems(cartItems);

            int uniqueProductCount = cartItems.Select(c => c.ProductID).Distinct().Count();

            return Json(new { success = true, totalItems = uniqueProductCount });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(c => c.ProductID == productId);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
                SaveCartItems(cartItems);

                int uniqueProductCount = cartItems.Select(c => c.ProductID).Distinct().Count();
                decimal subtotal = cartItems.Sum(i => i.UnitPrice * i.Quantity);
                return Json(new { success = true, totalItems = uniqueProductCount, subtotal });
            }

            return Json(new { success = false, message = "Product not found in cart." });
        }

        private void SaveCartItems(List<CartItem> cartItems)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true
            };

            var cartJson = JsonConvert.SerializeObject(cartItems);
            Response.Cookies.Append("Cart", cartJson, cookieOptions);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            Response.Cookies.Delete("Cart");
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var customerJson = HttpContext.Session.GetString("customer");
            if (string.IsNullOrEmpty(customerJson))
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            var customer = JsonConvert.DeserializeObject<Customer>(customerJson);

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Now,
                TotalAmount = request.TotalAmount,
                Status = "Pending",
                ShipAddress = customer.Address
            };

            var cartItems = GetCartItems();
            var orderDetails = cartItems.Select(item => new OrderDetail
            {
                ProductId = item.ProductID,
                Quantity = item.Quantity,
                Price = item.UnitPrice
            }).ToList();

            await _productService.CreateOrderAsync(order, orderDetails);
            await _signalRHub.Clients.All.SendAsync("NewOrderCreated", new
            {
                order.OrderId,
                CustomerName = customer.FullName,
                order.OrderDate,
                order.TotalAmount,
                order.Status,
                order.ShipAddress
            });

            ClearCart();
            return Json(new { success = true });
        }
    }
}
