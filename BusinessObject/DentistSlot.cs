using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class DentistSlot
{
    public int DentistSlotId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public bool? Status { get; set; }

    public int? RoomId { get; set; }

    public int? DentistId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? Dentist { get; set; }

    public virtual Room? Room { get; set; }
}
