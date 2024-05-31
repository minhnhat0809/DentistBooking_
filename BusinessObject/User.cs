using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public int? ClinicId { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<CheckupSchedule> CheckupScheduleCustomers { get; set; } = new List<CheckupSchedule>();

    public virtual ICollection<CheckupSchedule> CheckupScheduleDentists { get; set; } = new List<CheckupSchedule>();

    public virtual Clinic? Clinic { get; set; }

    public virtual ICollection<CustomerDentistSlot> CustomerDentistSlots { get; set; } = new List<CustomerDentistSlot>();

    public virtual ICollection<DentistSlot> DentistSlots { get; set; } = new List<DentistSlot>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
