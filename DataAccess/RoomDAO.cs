using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class RoomDAO
{
    private static RoomDAO instance = null;
            private static readonly object instanceLock = new object();
            private RoomDAO() { }
            public static RoomDAO Instance
            {
                get
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new RoomDAO();
                        }
                        return instance;
                    }
                }
            }
    
            public async Task<Room> getRooomByID(int? id)
            {
                var context = new BookingDentistDbContext();
                var room = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId  == id);
                return room;
            }
    
            public async Task<List<Room>> getAllCRooms()
            {
                var context = new BookingDentistDbContext();
                var roomList = await context.Rooms.ToListAsync();
                return roomList;
            }
    
            public async Task deleteRoom(Room room)
            {
                var context = new BookingDentistDbContext();
                room.Status = false;
                context.Entry<Room>(room).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
    
            public async Task createRoom(Room room)
            {
                var context = new BookingDentistDbContext();
                context.Rooms.Add(room);
                await context.SaveChangesAsync();
            }
    
            public async Task updateRoom(Room room)
            {
                var context = new BookingDentistDbContext();
                context.Entry<Room>(room).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
}