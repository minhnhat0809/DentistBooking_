using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class CustomerDentistSlot
{
    public int CustomerDentistSlotId { get; set; }

    public int? CustomerId { get; set; }

    public int? DentistSlotId { get; set; }

    public virtual User? Customer { get; set; }

    public virtual DentistSlot? DentistSlot { get; set; }
}
