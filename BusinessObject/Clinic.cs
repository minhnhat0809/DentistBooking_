using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Clinic
{
    public int ClinicId { get; set; }

    public string ClinicName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
