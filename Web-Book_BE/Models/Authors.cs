using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Authors
{
    public string AuthorId { get; set; } = null!;

    public string? AuthorName { get; set; }

    public string? Bio { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
