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

        public async Task<List<DentistSlot>> GetAllDentistSlotsByDentist(int id) => await DentistSlotDAO.Instance.getAllDentistSlotsByDentist(id);

        public async Task<DentistSlot> GetDentistSlotByID(int dentistSlotId) => await DentistSlotDAO.Instance.getDentistSlotByID(dentistSlotId);
    }
}
