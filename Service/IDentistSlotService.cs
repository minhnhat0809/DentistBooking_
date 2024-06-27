using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDentistSlotService
    {
        Task<List<DentistSlot>> GetAllDentistSlots();

        Task<List<DentistSlot>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate);

        Task<DentistSlot> GetDentistSlot(int dentistSlotId);
    }
}
