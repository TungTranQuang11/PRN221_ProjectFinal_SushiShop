using System;
using System.Collections.Generic;

namespace ProjectFinal.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
