using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class CheckupSchedule
{
    public int ScheduleId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public string? Notes { get; set; }

    public bool? Status { get; set; }

    public int? CustomerId { get; set; }

    public int? DentistId { get; set; }

    public int? CreateBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreateByNavigation { get; set; }

    public virtual User? Customer { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }
}
