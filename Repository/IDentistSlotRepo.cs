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

        Task<List<DentistSlot>> GetAllDentistSlotsByDentist(int id);

        Task<DentistSlot> GetDentistSlotByID(int dentistSlotId);
    }
}
