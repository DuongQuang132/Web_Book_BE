using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Orders
{
    public string OrdersId { get; set; } = null!;

    public string? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
