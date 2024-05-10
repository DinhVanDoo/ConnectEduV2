using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<DepositTransaction> DepositTransactions { get; set; } = new List<DepositTransaction>();

    public virtual ICollection<PurchaseTransaction> PurchaseTransactions { get; set; } = new List<PurchaseTransaction>();

    public virtual User User { get; set; } = null!;
}
