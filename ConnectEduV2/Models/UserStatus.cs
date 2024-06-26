﻿using System;
using System.Collections.Generic;

namespace ConnectEduV2.Models;

public partial class UserStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
