using System.Threading.Tasks;
using ProjectFinal.Models;
using ProjectFinal.Repositories;

namespace ProjectFinal.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;

        public AuthenticationService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> SignInAsync(string email, string password)
        {
            return await _customerRepository.GetCustomerByEmailAndPasswordAsync(email, password);
        }

        public async Task<bool> RegisterCustomerAsync(string fullname, string email, string password)
        {
            var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (existingCustomer != null) return false;

            var newCustomer = new Customer
            {
                FullName = fullname,
                Email = email,
                Password = password,
                Role = 0
            };

            await _customerRepository.AddCustomerAsync(newCustomer);
            return true;
        }
    }
}
