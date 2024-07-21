namespace BusinessObject.Result;

public class ListRoomResult
{
    public string Message { get; set; } = "";

    public List<Room>? Rooms { get; set; } = new List<Room>();
}