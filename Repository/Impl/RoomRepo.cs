using BusinessObject;
using DataAccess;

namespace Repository.Impl;

public class RoomRepo : IRoomRepo
{
    public async Task<List<Room>> GetAllRooms() => RoomDAO.Instance.getAllCRooms().Result;
    public Room? GetRoomById(int roomId) => RoomDAO.Instance.getRooomByID(roomId).Result;
}