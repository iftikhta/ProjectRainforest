using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectRainforest.Models
{
    public partial class ProductInfo
    {
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string ProductImg { get; set; }
        public double? ProductRating { get; set; }
        public DateTime DateAdded { get; set; }
        public int? RatingCount { get; set; }

        public virtual Product Product { get; set; }
    }
}
