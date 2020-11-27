using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectRainforest.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public int? VendorId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string CardNumber { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public int UserRole { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
