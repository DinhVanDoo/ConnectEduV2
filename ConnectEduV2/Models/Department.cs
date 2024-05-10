using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? SchoolId { get; set; }

    public int? DataStatus { get; set; }

    public virtual DataStatus? DataStatusNavigation { get; set; }

    public virtual School? School { get; set; }

    public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
