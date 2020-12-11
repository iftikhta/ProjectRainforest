using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Cart
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
