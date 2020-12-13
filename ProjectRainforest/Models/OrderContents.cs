using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class OrderContents
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double PricePaid { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
