using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class PrescriptionMedicine
{
    public int PrescriptionMedicineId { get; set; }

    public int? PrescriptionId { get; set; }

    public int? MedicineId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual Medicine? Medicine { get; set; }

    public virtual Prescription? Prescription { get; set; }
}
