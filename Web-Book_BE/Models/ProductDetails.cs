using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class ProductDetails
{
    public string ProductDetailId { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? PublishDate { get; set; }

    public int? PageCount { get; set; }

    public string? Language { get; set; }

    public virtual Product? Product { get; set; }
}
