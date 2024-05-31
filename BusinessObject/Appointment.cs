using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateOnly Date { get; set; }

    public string? Diagnosis { get; set; }

    public bool? Status { get; set; }

    public int? DentistSlotId { get; set; }

    public int? MedicalRecordId { get; set; }

    public virtual DentistSlot? DentistSlot { get; set; }

    public virtual MedicalRecord? MedicalRecord { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<ServiceAppointment> ServiceAppointments { get; set; } = new List<ServiceAppointment>();
}
