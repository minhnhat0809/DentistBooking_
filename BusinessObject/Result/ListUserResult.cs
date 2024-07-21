using BusinessObject.DTO;

namespace BusinessObject.Result;

public class ListUserResult
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();

    public string Message { get; set; } = "";
}