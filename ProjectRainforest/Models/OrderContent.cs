using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectRainforest.Models
{
    public partial class OrderContent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double PricePaid { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
