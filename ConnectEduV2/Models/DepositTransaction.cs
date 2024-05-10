using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class DepositTransaction
{
    public int Id { get; set; }

    public int? WalletId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }

    public int? PaymentStatusId { get; set; }

    public virtual PaymentStatus? PaymentStatus { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
