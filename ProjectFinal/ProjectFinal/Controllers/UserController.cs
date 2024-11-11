using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectFinal.Models;
using ProjectFinal.Services;

namespace ProjectFinal.Controllers
{
    public class UserController : Controller
    {
        private readonly ICustomerService _customerService;

        public UserController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private Customer? GetAuthenticatedCustomer()
        {
            var customerJson = HttpContext.Session.GetString("customer");
            if (string.IsNullOrEmpty(customerJson))
            {
                return null;
            }

            var customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            if (customer?.Role == 1)
            {
                RedirectToAction("AccessDenied", "Authentication");
            }

            return customer;
        }

        public IActionResult Profile()
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(Customer updatedCustomer)
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            if (!string.Equals(customer.Email?.Trim(), updatedCustomer.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = "Email cannot be changed.";
                return View(customer);
            }

            customer.FullName = updatedCustomer.FullName;
            customer.Phone = updatedCustomer.Phone;
            customer.Address = updatedCustomer.Address;

            await _customerService.UpdateCustomerAsync(customer);

            HttpContext.Session.SetString("customer", JsonConvert.SerializeObject(customer));
            TempData["Success"] = "Profile updated successfully!";

            return RedirectToAction("Profile");
        }

        public IActionResult ChangePassword()
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            var success = await _customerService.ChangePasswordAsync(customer.CustomerId, currentPassword, newPassword);
            return Json(new { success, error = success ? null : "Current password is incorrect." });
        }

        public async Task<IActionResult> OrderHistory(int pageNumber = 1)
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            const int pageSize = 5;
            var paginatedOrders = await _customerService.GetCustomerOrdersAsync(customer.CustomerId, pageNumber, pageSize);
            return View(paginatedOrders);
        }


        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId, int pageNumber)
        {
            var customer = GetAuthenticatedCustomer();
            if (customer == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            var success = await _customerService.CancelOrderAsync(customer.CustomerId, orderId);
            if (!success)
            {
                ViewBag.Error = "Order not found or you do not have permission to cancel this order.";
            }

            return RedirectToAction("OrderHistory", new { pageNumber });
        }
    }
}
