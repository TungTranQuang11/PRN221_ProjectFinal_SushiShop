using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectFinal.Models;
using ProjectFinal.Services;
using System.Threading.Tasks;

namespace ProjectFinal.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        private IActionResult? Authenticated()
        {
            if (HttpContext.Session.TryGetValue("customer", out _))
            {
                var customerJson = HttpContext.Session.GetString("customer");
                var customer = JsonConvert.DeserializeObject<Customer>(customerJson);
                if (customer != null && customer.Role == 1)
                {
                    return RedirectToAction("ManageProducts", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            return null;
        }

        public IActionResult SignIn()
        {
            var result = Authenticated();
            if (result != null)
            {
                return result;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var customer = await _authService.SignInAsync(email, password);

            if (customer != null)
            {
                var customerJson = JsonConvert.SerializeObject(customer);
                HttpContext.Session.SetString("customer", customerJson);

                return customer.Role == 1 ? RedirectToAction("ManageProducts", "Admin") : RedirectToAction("Index", "Home");
            }

            ViewBag.Email = email;
            ViewBag.Error = "Invalid email or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer");
            return RedirectToAction("SignIn");
        }

        public IActionResult SignUp()
        {
            var result = Authenticated();
            if (result != null)
            {
                return result;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string fullname, string email, string password)
        {
            var isRegistered = await _authService.RegisterCustomerAsync(fullname, email, password);

            if (!isRegistered)
            {
                ViewBag.Error = "This email is already registered.";
                ViewBag.FullName = fullname;
                ViewBag.Email = email;
                return View();
            }

            return RedirectToAction("SignIn");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
