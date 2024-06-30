using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class DentistSlotRepo : IDentistSlotRepo
    {
        public async Task<List<DentistSlot>> GetAllDentistSlots() => await DentistSlotDAO.Instance.getAllDentistSlots();

        public async Task<List<DentistSlot>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate) => await DentistSlotDAO.Instance.getAllDentistSlotsByDentist(id, selectedDate);

        public async Task<DentistSlot> GetDentistSlotByID(int dentistSlotId) => await DentistSlotDAO.Instance.getDentistSlotByID(dentistSlotId);
    }
}
