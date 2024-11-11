using System.Threading.Tasks;
using ProjectFinal.Models;
using System.Collections.Generic;

namespace ProjectFinal.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByEmailAndPasswordAsync(string email, string password);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task UpdateCustomerAsync(Customer customer);
        Task<bool> ChangePasswordAsync(int customerId, string currentPassword, string newPassword);
        Task<PaginatedList<Order>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize);
        Task<bool> CancelOrderAsync(int customerId, int orderId);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}
