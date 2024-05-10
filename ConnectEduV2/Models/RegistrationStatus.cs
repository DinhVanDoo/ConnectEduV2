using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class RegistrationStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ClassRegistration> ClassRegistrations { get; set; } = new List<ClassRegistration>();
}
