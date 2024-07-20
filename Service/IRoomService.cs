using BusinessObject;
using BusinessObject.Result;

namespace Service;

public interface IRoomService
{
    Task<ListRoomResult> GetAllRooms();
}