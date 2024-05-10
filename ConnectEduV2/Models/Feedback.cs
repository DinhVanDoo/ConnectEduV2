using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Comment { get; set; }

    public int? UserId { get; set; }

    public int? RegistrationId { get; set; }

    public int? ClassId { get; set; }

    public int? EvaluateId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Evaluate? Evaluate { get; set; }

    public virtual ClassRegistration? Registration { get; set; }

    public virtual User? User { get; set; }
}
