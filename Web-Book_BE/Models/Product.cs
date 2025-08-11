using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string? ProductName { get; set; }

    public string? AuthorId { get; set; }

    public string? CategoriesId { get; set; }

    public decimal? Price { get; set; }

    public string? Discount { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Authors? Author { get; set; }

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual Categories? Categories { get; set; }

    public virtual ICollection<ProductDetails> ProductDetails { get; set; } = new List<ProductDetails>();
}
