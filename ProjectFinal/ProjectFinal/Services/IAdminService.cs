using ProjectFinal.Models;
using System.Threading.Tasks;

namespace ProjectFinal.Services
{
    public interface IAdminService
    {
        Task<PaginatedList<Product>> GetPaginatedProductsAsync(string searchQuery, int? categoryId, int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<PaginatedList<Order>> GetPaginatedOrdersAsync(int pageNumber, DateTime? startDate, DateTime? endDate, int pageSize);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task<PaginatedList<Customer>> GetPaginatedCustomersAsync(string searchQuery, int pageNumber, int pageSize);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task UpdateCustomerAsync(Customer customer);
        Task<List<Category>> GetCategoriesAsync();
    }
}
