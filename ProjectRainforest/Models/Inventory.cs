using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Inventory
    {
        public int ProductId { get; set; }
        public int Count { get; set; }

        public virtual Product Product { get; set; }
    }
}
