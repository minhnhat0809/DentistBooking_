﻿using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public string? Diagnosis { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public int? DentistSlotId { get; set; }

    public int? CustomerId { get; set; }

    public int? ServiceId { get; set; }

    public int? MedicalRecordId { get; set; }

    public int? CreateBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreateByNavigation { get; set; }

    public virtual User? Customer { get; set; }

    public virtual DentistSlot? DentistSlot { get; set; }

    public virtual MedicalRecord? MedicalRecord { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual Service? Service { get; set; }
}
