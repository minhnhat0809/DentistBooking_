using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public string? Diagnosis { get; set; }

    public string? Status { get; set; }

    public int? DentistSlotId { get; set; }

    public int? CustomerId { get; set; }

    public int? ServiceId { get; set; }

    public int? MedicalRecordId { get; set; }

    public virtual User? Customer { get; set; }
    [JsonIgnore]
    public virtual DentistSlot? DentistSlot { get; set; }

    public virtual MedicalRecord? MedicalRecord { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual Service? Service { get; set; }
}
