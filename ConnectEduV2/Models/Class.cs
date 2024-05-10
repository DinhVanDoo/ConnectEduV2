using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class Class
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Describe { get; set; }

    public int? SubjectId { get; set; }

    public int? UserId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? ClassStatusId { get; set; }

    public string? CoursePath { get; set; }

    public int? SeatNumber { get; set; }

    public virtual ICollection<ClassChat> ClassChats { get; set; } = new List<ClassChat>();

    public virtual ICollection<ClassRegistration> ClassRegistrations { get; set; } = new List<ClassRegistration>();

    public virtual ClassStatus? ClassStatus { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Subject? Subject { get; set; }

    public virtual User? User { get; set; }
}
