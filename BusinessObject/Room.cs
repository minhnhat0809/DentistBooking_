using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Room
{
    public int RoomId { get; set; }

    public int? RoomNumber { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<DentistSlot> DentistSlots { get; set; } = new List<DentistSlot>();
}
