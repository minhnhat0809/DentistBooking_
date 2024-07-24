using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Result;

namespace Service
{
    public interface IDentistSlotService
    {
        Task<List<DentistSlotDto>> GetAllDentistSlots();

        Task<List<DentistSlotDto>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate);

        Task<DentistSlotDto> GetDentistSlotById(int dentistSlotId);

        Task<DentistSlotResult> CreateDentistSlot(int dentistId, DateTime timeStart, DateTime timeEnd, int RoomId);

        ListDentistSlotResult GetDentistSlotByServiceAndDateTime(int serviceId, DateTime timeStart);
        
        /*ListDentistSlotResult GetDentistSlotByServiceAndDate(int serviceId, DateTime timeStart);*/

        ListDentistSlotResult GetDentistSlotForAppointment(List<DentistSlot> dentistSlots, int dentistSlotId);

        DentistSlotResult GetDentistSlotByAppointmentTimeStart(DateTime TimeStart, int dentistId);
    }
}
