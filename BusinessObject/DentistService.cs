using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class DentistService
{
    public int DentistServiceId { get; set; }

    public int? ServiceId { get; set; }

    public int? DentistId { get; set; }

    public bool? Status { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual Service? Service { get; set; }
}
