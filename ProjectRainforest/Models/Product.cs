using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectRainforest.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
