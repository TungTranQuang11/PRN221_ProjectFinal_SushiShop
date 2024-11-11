using ProjectFinal.Models;
using ProjectFinal.Repositories;
using System;
using System.Threading.Tasks;

namespace ProjectFinal.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<PaginatedList<Product>> GetPaginatedProductsAsync(string searchQuery, int? categoryId, int pageNumber, int pageSize)
        {
            return await _adminRepository.GetProductsAsync(searchQuery, categoryId, pageNumber, pageSize);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _adminRepository.GetProductByIdAsync(productId);
        }

        public async Task AddProductAsync(Product product)
        {
            await _adminRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _adminRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _adminRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                await _adminRepository.DeleteProductAsync(product);
            }
        }

        public async Task<PaginatedList<Order>> GetPaginatedOrdersAsync(int pageNumber, DateTime? startDate, DateTime? endDate, int pageSize)
        {
            return await _adminRepository.GetOrdersAsync(pageNumber, startDate, endDate, pageSize);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _adminRepository.GetOrderByIdAsync(orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _adminRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _adminRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                await _adminRepository.DeleteOrderAsync(order);
            }
        }

        public async Task<PaginatedList<Customer>> GetPaginatedCustomersAsync(string searchQuery, int pageNumber, int pageSize)
        {
            return await _adminRepository.GetCustomersAsync(searchQuery, pageNumber, pageSize);
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _adminRepository.GetCustomerByIdAsync(customerId);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _adminRepository.UpdateCustomerAsync(customer);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _adminRepository.GetCategoriesAsync();
        }
    }
}
