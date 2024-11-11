using ProjectFinal.Models;

namespace ProjectFinal.Services
{
    public interface IProductService
    {
        Task<PaginatedList<Product>> GetProductsAsync(int pageIndex, int pageSize, string sortOrder, string searchQuery, int? categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task UpdateProductStockAsync(int productId, int quantity);
        Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails);
        Task<Product?> GetProductByIdWithCategoryAsync(int productId);
    }
}
