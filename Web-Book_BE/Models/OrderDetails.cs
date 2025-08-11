using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class OrderDetails
{
    public string OrderDetailId { get; set; } = null!;

    public string? OrdersId { get; set; }

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Orders? Orders { get; set; }
}
