using System;
using System.Collections.Generic;

namespace Web_Book_BE.Models;

public partial class PaymentHistory
{
    public string PaymentHistoryId { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }

    public string? PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public virtual Payment? Payment { get; set; }
}
