using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<DentistService> DentistServices { get; set; } = new List<DentistService>();

    public virtual ICollection<ServiceAppointment> ServiceAppointments { get; set; } = new List<ServiceAppointment>();
}
