using Microsoft.EntityFrameworkCore;
using ProjectFinal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinal.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FoodShopContext _context;

        public AdminRepository(FoodShopContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Product>> GetProductsAsync(string searchQuery, int? categoryId, int pageNumber, int pageSize)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(searchQuery));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            return await PaginatedList<Product>.CreateAsync(productsQuery.OrderBy(p => p.ProductId), pageNumber, pageSize);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Order>> GetOrdersAsync(int pageNumber, DateTime? startDate, DateTime? endDate, int pageSize)
        {
            var ordersQuery = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate);

            if (startDate.HasValue && endDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value && o.OrderDate <= endDate.Value).OrderByDescending(o => o.OrderDate);
            }

            return await PaginatedList<Order>.CreateAsync(ordersQuery, pageNumber, pageSize);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Customer>> GetCustomersAsync(string searchQuery, int pageNumber, int pageSize)
        {
            var customersQuery = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                customersQuery = customersQuery.Where(c => c.FullName.Contains(searchQuery) || c.Email.Contains(searchQuery));
            }

            return await PaginatedList<Customer>.CreateAsync(customersQuery, pageNumber, pageSize);
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
