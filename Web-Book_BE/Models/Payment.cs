using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class Payment
{
    public string PaymentId { get; set; } = null!;

    public string? OrdersId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Status { get; set; }

    public string? Noted { get; set; }

    public virtual Orders? Orders { get; set; }

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
}
