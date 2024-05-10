using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Evaluate
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
