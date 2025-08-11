using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Customers
{
    public string CustomerId { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
