using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Product
    {
        public Product()
        {
            Cart = new HashSet<Cart>();
        }

        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ProductInfo ProductInfo { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
    }
}
