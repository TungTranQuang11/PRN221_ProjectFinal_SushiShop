using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectFinal.Models;
using System.Linq;

namespace ProjectFinal.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FoodShopContext _context;

        public CustomerRepository(FoodShopContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
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

        public async Task<bool> ChangePasswordAsync(int customerId, string currentPassword, string newPassword)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || customer.Password != currentPassword)
            {
                return false;
            }
            customer.Password = newPassword;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedList<Order>> GetCustomerOrdersAsync(int customerId, int pageNumber, int pageSize)
        {
            var ordersQuery = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product);

            return await PaginatedList<Order>.CreateAsync(ordersQuery, pageNumber, pageSize);
        }


        public async Task<bool> CancelOrderAsync(int customerId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == customerId);
            if (order == null) return false;

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
