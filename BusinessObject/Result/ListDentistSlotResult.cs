namespace BusinessObject.Result;

public class ListDentistSlotResult
{
    public List<DentistSlot>? DentistSlots { get; set; } = new List<DentistSlot>();
    public string Message = "";
}