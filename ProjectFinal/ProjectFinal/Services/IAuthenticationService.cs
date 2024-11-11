using System.Threading.Tasks;
using ProjectFinal.Models;

namespace ProjectFinal.Services
{
    public interface IAuthenticationService
    {
        Task<Customer> SignInAsync(string email, string password);
        Task<bool> RegisterCustomerAsync(string fullname, string email, string password);
    }
}
