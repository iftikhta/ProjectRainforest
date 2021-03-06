﻿using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            Product = new HashSet<Product>();
        }

        public int VendorId { get; set; }
        public string VendorTitle { get; set; }
        public string VendorDescription { get; set; }
        public double? VendorRatingAvg { get; set; }
        public string VendorImg { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
