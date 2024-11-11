using ProjectFinal.Models;
using ProjectFinal.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectFinal.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PaginatedList<Product>> GetProductsAsync(int pageIndex, int pageSize, string sortOrder, string searchQuery, int? categoryId)
        {
            return await _productRepository.GetProductsAsync(pageIndex, pageSize, sortOrder, searchQuery, categoryId);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _productRepository.GetCategoriesAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

        public async Task UpdateProductStockAsync(int productId, int quantity)
        {
            await _productRepository.UpdateProductStockAsync(productId, quantity);
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            await _productRepository.CreateOrderAsync(order, orderDetails);
        }

        public async Task<Product?> GetProductByIdWithCategoryAsync(int productId)
        {
            return await _productRepository.GetProductByIdWithCategoryAsync(productId);
        }
    }
}
