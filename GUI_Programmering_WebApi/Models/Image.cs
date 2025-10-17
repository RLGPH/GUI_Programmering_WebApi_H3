using System;
using System.Collections.Generic;

namespace GUI_Programmering_WebApi.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }

        public required string ImageName { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
