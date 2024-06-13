using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ServiceAppointment
{
    public int ServiceAppointmentId { get; set; }

    public int? ServiceId { get; set; }

    public int? AppointmentId { get; set; }

    public bool? Status { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Service? Service { get; set; }
}
