using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class DataStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<School> Schools { get; set; } = new List<School>();

    public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
