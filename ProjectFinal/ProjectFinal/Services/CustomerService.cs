using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectFinal.Models;
using ProjectFinal.Repositories;

namespace ProjectFinal.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepository.GetCustomerByIdAsync(customerId);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task<bool> ChangePasswordAsync(int customerId, string currentPassword, string newPassword)
        {
            return await _customerRepository.ChangePasswordAsync(customerId, currentPassword, newPassword);
        }

        public async Task<PaginatedList<Order>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize)
        {
            return await _customerRepository.GetCustomerOrdersAsync(customerId, pageNumber, pageSize);
        }

        public async Task<bool> CancelOrderAsync(int customerId, int orderId)
        {
            return await _customerRepository.CancelOrderAsync(customerId, orderId);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId) 
        {
            return await _customerRepository.DeleteCustomerAsync(customerId);
        }

        public async Task AddCustomerAsync(Customer customer) 
        {
            await _customerRepository.AddCustomerAsync(customer);
        }
    }
}
