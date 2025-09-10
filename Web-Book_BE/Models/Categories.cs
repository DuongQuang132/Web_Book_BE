using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Categories
{
    public string Categories_ID { get; set; } // hoặc dùng [Key] nếu cần
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
