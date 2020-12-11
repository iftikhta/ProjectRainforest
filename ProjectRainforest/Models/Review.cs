using System;
using System.Collections.Generic;

namespace ProjectRainforest.Models
{
    public partial class Review
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDescription { get; set; }
        public int ReviewRating { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
