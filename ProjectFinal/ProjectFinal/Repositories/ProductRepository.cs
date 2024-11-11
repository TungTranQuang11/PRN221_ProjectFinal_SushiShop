using Microsoft.EntityFrameworkCore;
using ProjectFinal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFinal.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FoodShopContext _context;

        public ProductRepository(FoodShopContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Product>> GetProductsAsync(int pageIndex, int pageSize, string sortOrder, string searchQuery, int? categoryId)
        {
            var products = _context.Products.Where(p => p.Status != true).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.ProductName.Contains(searchQuery));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            products = sortOrder switch
            {
                "PriceHighToLow" => products.OrderByDescending(p => p.Price),
                "PriceLowToHigh" => products.OrderBy(p => p.Price),
                _ => products.OrderBy(p => p.ProductId),
            };

            return await PaginatedList<Product>.CreateAsync(products, pageIndex, pageSize);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateProductStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.QuantityInStock -= quantity;
                if (product.QuantityInStock < 0)
                    throw new InvalidOperationException($"Not enough stock for product {product.ProductName}.");
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.OrderId;

                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.QuantityInStock -= detail.Quantity;
                    _context.Products.Update(product);
                }
            }

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByIdWithCategoryAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)  
                .FirstOrDefaultAsync(p => p.ProductId == productId);

        }
    }
}
