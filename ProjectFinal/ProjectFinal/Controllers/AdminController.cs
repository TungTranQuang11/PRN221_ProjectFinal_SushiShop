using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ProjectFinal.Hubs;
using ProjectFinal.Models;
using ProjectFinal.Services;

namespace ProjectFinal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public AdminController(IAdminService adminService, ICustomerService customerService, IProductService productService, IWebHostEnvironment webHostEnvironment, IHubContext<SignalrServer> signalRHub)

        {
            _adminService = adminService;
            _customerService = customerService;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _signalRHub = signalRHub;
        }

        private IActionResult? CheckCustomerAccess()
        {
            var customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                var existingCustomer = JsonConvert.DeserializeObject<Customer>(customerJson);
                if (existingCustomer.Role == 0)
                {
                    return RedirectToAction("AccessDenied", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Authentication");
            }

            return null;
        }

        public async Task<IActionResult> ManageProducts(string searchQuery, int? categoryId, int pageNumber = 1)
        {
            var accessResult = CheckCustomerAccess();
            if (accessResult != null)
            {
                return accessResult;
            }

            ViewBag.Categories = await _adminService.GetCategoriesAsync();

            var paginatedProducts = await _adminService.GetPaginatedProductsAsync(searchQuery, categoryId, pageNumber, 5);

            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentSearch = searchQuery;

            return View(paginatedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(int productId, string productName, string description, decimal price, int quantityInStock, int categoryId, bool status, IFormFile imageFile, int pageNumber)
        {
            var product = await _adminService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            product.ProductName = productName;
            product.Description = description;
            product.Price = price;
            product.QuantityInStock = quantityInStock;
            product.CategoryId = categoryId;
            product.Status = status;

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/product");
                string uniqueFileName = imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.Image = uniqueFileName;
            }

            await _adminService.UpdateProductAsync(product);

            await _signalRHub.Clients.All.SendAsync("ProductUpdated", new
            {
                productId = product.ProductId,
                productName = product.ProductName,
                categoryId = product.CategoryId,
                categoryName = product.Category?.CategoryName,
                price = product.Price,
                quantityInStock = product.QuantityInStock,
                status = product.Status,
                image = product.Image,
                description = product.Description
            });

            return RedirectToAction("ManageProducts", new { pageNumber });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string productName, string description, decimal price, int quantityInStock, int categoryId, bool status, IFormFile imageFile, int pageNumber)
        {
            var newProduct = new Product
            {
                ProductName = productName,
                Description = description,
                Price = price,
                QuantityInStock = quantityInStock,
                CategoryId = categoryId,
                Status = status
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/product");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                newProduct.Image = uniqueFileName;
            }

            await _adminService.AddProductAsync(newProduct);
            newProduct = await _productService.GetProductByIdWithCategoryAsync(newProduct.ProductId);

            await _signalRHub.Clients.All.SendAsync("ProductAdded", new
            {
                productId = newProduct.ProductId,
                productName = newProduct.ProductName,
                categoryName = newProduct.Category?.CategoryName,
                price = newProduct.Price,
                quantityInStock = newProduct.QuantityInStock,
                status = newProduct.Status,
                image = newProduct.Image
            });

            return RedirectToAction("ManageProducts", new { pageNumber });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id, int pageNumber)
        {
            var product = await _adminService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _adminService.DeleteProductAsync(id);

            await _signalRHub.Clients.All.SendAsync("ProductDeleted", id);

            return RedirectToAction("ManageProducts", new { pageNumber });
        }

        public async Task<IActionResult> ManageOrders(int pageNumber = 1, DateTime? startDate = null, DateTime? endDate = null)
        {
            var accessResult = CheckCustomerAccess();
            if (accessResult != null)
            {
                return accessResult;
            }

            const int pageSize = 5;

            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            {
                ViewBag.ErrorMessage = "Start date cannot be greater than end date.";
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
                return View(new PaginatedList<Order>(new List<Order>(), 0, pageNumber, pageSize));
            }

            var paginatedOrders = await _adminService.GetPaginatedOrdersAsync(pageNumber, startDate, endDate, pageSize);

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            if (!paginatedOrders.Any())
            {
                ViewBag.Message = "No orders found for the selected date range.";
            }

            return View(paginatedOrders);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(int orderId, string status, string shipAddress, int pageNumber)
        {
            var order = await _adminService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound();

            order.Status = status;
            order.ShipAddress = shipAddress;

            await _adminService.UpdateOrderAsync(order);

            await _signalRHub.Clients.All.SendAsync("OrderUpdated", new
            {
                orderId = order.OrderId,
                status = order.Status,
                shipAddress = order.ShipAddress,
                customerName = order.Customer?.FullName,
            });

            return RedirectToAction("ManageOrders", new { pageNumber });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id, int pageNumber)
        {
            var order = await _adminService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _adminService.DeleteOrderAsync(id);

            await _signalRHub.Clients.All.SendAsync("OrderDeleted", id);

            return RedirectToAction("ManageOrders", new { pageNumber });
        }

        public async Task<IActionResult> ManageAccounts(string searchQuery)
        {
            var accessResult = CheckCustomerAccess();
            if (accessResult != null)
            {
                return accessResult;
            }

            var paginatedCustomers = await _adminService.GetPaginatedCustomersAsync(searchQuery, 1, 10);

            ViewBag.Customers = paginatedCustomers;
            ViewBag.CurrentSearch = searchQuery;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(int customerId, string fullName, string email, string password, string phone, string address, int role)
        {
            var customer = await _adminService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound();

            customer.FullName = fullName;
            customer.Email = email;
            customer.Phone = phone;
            customer.Address = address;
            customer.Role = role;
            customer.Password = password;

            await _adminService.UpdateCustomerAsync(customer);

            await _signalRHub.Clients.All.SendAsync("CustomerUpdated", customer);

            return RedirectToAction("ManageAccounts");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(string fullName, string email, string phone, string address, string password, int role)
        {
            var newCustomer = new Customer
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Address = address,
                Password = password,
                Role = role
            };

            await _customerService.AddCustomerAsync(newCustomer);

            await _signalRHub.Clients.All.SendAsync("CustomerAdded", newCustomer);

            return RedirectToAction("ManageAccounts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            // Try to delete the customer using the service
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var result = await _customerService.DeleteCustomerAsync(customerId);

            if (result)
            {
                await _signalRHub.Clients.All.SendAsync("CustomerDeleted", customerId);
                return RedirectToAction("ManageAccounts");
            }
            return RedirectToAction("ManageAccounts");
        }
    }
}
