using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Semester
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DepartmentId { get; set; }

    public int? SchoolId { get; set; }

    public int? DataStatusId { get; set; }

    public virtual DataStatus? DataStatus { get; set; }

    public virtual Department? Department { get; set; }

    public virtual School? School { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
