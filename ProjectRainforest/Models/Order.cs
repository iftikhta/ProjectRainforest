using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset DatePlaced { get; set; }
        public DateTimeOffset? DateUserCancelled { get; set; }
        public DateTimeOffset? DateVendorCancelled { get; set; }
        public DateTimeOffset? DateFulfilled { get; set; }
        public string OrderStatus { get; set; }
        public double Total { get; set; }

        public virtual User User { get; set; }
    }
}
