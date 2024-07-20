using BusinessObject;
using BusinessObject.Result;
using Repository;

namespace Service.Impl;

public class RoomService : IRoomService
{
    private readonly IRoomRepo _roomRepo;
    
    public RoomService(IRoomRepo roomRepo)
    {
        _roomRepo = roomRepo;
    }
    
    public async Task<ListRoomResult> GetAllRooms()
    {
        ListRoomResult listRoomResult = new ListRoomResult();
        try
        {
            List<Room> rooms = await _roomRepo.GetAllRooms();
            listRoomResult.Rooms = rooms;
            listRoomResult.Message = "Success";
            
            return listRoomResult;
        }
        catch (Exception e)
        {
            listRoomResult.Rooms = null;
            listRoomResult.Message = e.Message;
            return listRoomResult;
        }
    }
}