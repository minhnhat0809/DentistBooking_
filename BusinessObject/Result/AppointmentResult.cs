using BusinessObject.DTO;

namespace BusinessObject.Result;

public class AppointmentResult
{
    public AppointmentDto Appointment { get; set; } = new AppointmentDto();

    public string Message { get; set; } = "";
}