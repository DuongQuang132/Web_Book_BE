using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual Customers? Customer { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
