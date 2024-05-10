using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class RevenueSharing
{
    public int Id { get; set; }

    public int? PurchaseId { get; set; }

    public int? UserId { get; set; }

    public decimal? MentorReceived { get; set; }

    public decimal? ProjectReceived { get; set; }

    public virtual PurchaseTransaction? Purchase { get; set; }

    public virtual User? User { get; set; }
}
