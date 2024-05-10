using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public int? SchoolId { get; set; }

    public int? DepartmentId { get; set; }

    public string? ScoreboardPhoto { get; set; }

    public string? Phone { get; set; }

    public string? FacebookPath { get; set; }

    public int? RoleId { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<ClassRegistration> ClassRegistrations { get; set; } = new List<ClassRegistration>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RevenueSharing> RevenueSharings { get; set; } = new List<RevenueSharing>();

    public virtual Role? Role { get; set; }

    public virtual School? School { get; set; }

    public virtual UserStatus? Status { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
