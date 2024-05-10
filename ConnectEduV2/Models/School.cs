using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class School
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public int DataStatusId { get; set; }

    public virtual DataStatus DataStatus { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
