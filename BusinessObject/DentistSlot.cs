using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class DentistSlot
{
    public int DentistSlotId { get; set; }

    public DateTime DateTime { get; set; }

    public bool? Status { get; set; }

    public int? DentistId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<CustomerDentistSlot> CustomerDentistSlots { get; set; } = new List<CustomerDentistSlot>();

    public virtual User? Dentist { get; set; }
}
