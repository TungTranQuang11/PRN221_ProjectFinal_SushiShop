using ProjectFinal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinal.Repositories
{
    public interface IAdminRepository
    {
        Task<PaginatedList<Product>> GetProductsAsync(string searchQuery, int? categoryId, int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<PaginatedList<Order>> GetOrdersAsync(int pageNumber, DateTime? startDate, DateTime? endDate, int pageSize);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task<PaginatedList<Customer>> GetCustomersAsync(string searchQuery, int pageNumber, int pageSize);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task UpdateCustomerAsync(Customer customer);
        Task<List<Category>> GetCategoriesAsync();
    }
}
