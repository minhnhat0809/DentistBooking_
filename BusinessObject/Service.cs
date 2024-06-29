using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DentistService> DentistServices { get; set; } = new List<DentistService>();
}
