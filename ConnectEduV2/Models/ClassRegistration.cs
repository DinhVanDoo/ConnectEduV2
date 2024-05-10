using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class ClassRegistration
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? ClassId { get; set; }

    public DateTime? Date { get; set; }

    public int? RegistrationStatusId { get; set; }

    public int? PaymentStatusId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<ClassChat> ClassChats { get; set; } = new List<ClassChat>();

    public virtual Feedback? Feedback { get; set; }

    public virtual PaymentStatus? PaymentStatus { get; set; }

    public virtual ICollection<PurchaseTransaction> PurchaseTransactions { get; set; } = new List<PurchaseTransaction>();

    public virtual RegistrationStatus? RegistrationStatus { get; set; }

    public virtual User User { get; set; } = null!;
}
