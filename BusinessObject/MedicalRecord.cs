using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class MedicalRecord
{
    public int MediaRecordId { get; set; }

    public DateOnly Date { get; set; }

    public string? Diagnosis { get; set; }

    public bool? Status { get; set; }

    public int? CustomerId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? Customer { get; set; }
}
