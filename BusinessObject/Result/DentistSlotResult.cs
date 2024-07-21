using BusinessObject.DTO;

namespace BusinessObject.Result;

public class DentistSlotResult
{
    public DentistSlotDto DentistSlot { get; set; } = new DentistSlotDto();

    public string Message { get; set; } = "";
}