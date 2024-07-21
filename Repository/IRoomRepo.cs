using BusinessObject;

namespace Repository;

public interface IRoomRepo
{
    Task<List<Room>> GetAllRooms();

    Room? GetRoomById(int roomId);
}