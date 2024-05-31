using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public DateOnly Date { get; set; }

    public string? Diagnosis { get; set; }

    public decimal? Total { get; set; }

    public bool? Status { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();
}
