using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IDentistSlotRepo
    {
        Task<List<DentistSlot>> GetAllDentistSlots();

        Task<List<DentistSlot>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate);
        
        Task<List<DentistSlot>> GetAllDentistSlotsByRoomAndDate(int roomId, DateTime selectedDate);

        Task<DentistSlot> GetDentistSlotByID(int dentistSlotId);

        Task CreateDentistSlot(DentistSlot dentistSlot);

        List<DentistSlot> GetAllDentistSlotByServiceAndTimeStart(int serviceId, DateTime timeStart);

        List<DentistSlot> GetAllDentistSlotByServiceAndDate(int serviceId, DateTime timeStart);
    }
}
