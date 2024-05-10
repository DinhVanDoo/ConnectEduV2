using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public int? SchoolId { get; set; }

    public int? DerpartmentId { get; set; }

    public int? SemesterId { get; set; }

    public int? DataStatusId { get; set; }

    public string? Describe { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual DataStatus? DataStatus { get; set; }

    public virtual Department? Derpartment { get; set; }

    public virtual School? School { get; set; }

    public virtual Semester? Semester { get; set; }
}
