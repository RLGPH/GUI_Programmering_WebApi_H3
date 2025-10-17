using System;
using System.Collections.Generic;

namespace GUI_Programmering_WebApi.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public required string ProductName { get; set; }
    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public int ImageId {  get; set; } 

    public int CategoryId { get; set; }

    public virtual Image Image { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
