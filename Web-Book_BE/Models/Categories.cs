using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Categories
{
    public string CategoriesId { get; set; } = null!;

    public string? CategoriesName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
