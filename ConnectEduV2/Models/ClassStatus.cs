using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class ClassStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
