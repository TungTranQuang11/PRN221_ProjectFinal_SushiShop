using ProjectFinal.Models;

namespace ProjectFinal.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task UpdateCustomerAsync(Customer customer);
        Task<bool> ChangePasswordAsync(int customerId, string currentPassword, string newPassword);
        Task<PaginatedList<Order>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize);
        Task<bool> CancelOrderAsync(int customerId, int orderId);
        Task<bool> DeleteCustomerAsync(int customerId);
        Task AddCustomerAsync(Customer customer);
    }
}
