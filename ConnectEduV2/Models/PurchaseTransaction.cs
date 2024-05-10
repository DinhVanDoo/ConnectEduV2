using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class PurchaseTransaction
{
    public int Id { get; set; }

    public int? RegistrationId { get; set; }

    public int? WalletId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }

    public int? PaymentStatus { get; set; }

    public virtual PaymentStatus? PaymentStatusNavigation { get; set; }

    public virtual ClassRegistration? Registration { get; set; }

    public virtual RevenueSharing? RevenueSharing { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
