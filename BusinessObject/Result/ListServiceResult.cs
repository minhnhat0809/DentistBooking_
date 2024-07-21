using BusinessObject.DTO;

namespace BusinessObject.Result;

public class ListServiceResult
{
    public List<ServiceDto> Services { get; set; } = new List<ServiceDto>();

    public string Message { get; set; } = "";
}