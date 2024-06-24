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

        Task<List<DentistSlot>> GetAllDentistSlotsByDentist(int id);

        Task<DentistSlot> GetDentistSlot(int dentistSlotId);
    }
}
