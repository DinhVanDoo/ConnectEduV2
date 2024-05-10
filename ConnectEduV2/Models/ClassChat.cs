using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class ClassChat
{
    public int Id { get; set; }

    public string? ChatContent { get; set; }

    public int? ClassRegistrationId { get; set; }

    public int? ClassId { get; set; }

    public DateTime? ChatDate { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ClassRegistration? ClassRegistration { get; set; }
}
