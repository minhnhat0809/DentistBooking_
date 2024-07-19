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

        public Task<List<DentistSlot>> GetAllDentistSlotsByRoomAndDate(int roomId, DateTime selectedDate) =>
            DentistSlotDAO.Instance.getAllDentistSlotsByRoomAndDate(roomId, selectedDate);

        public async Task<DentistSlot> GetDentistSlotByID(int dentistSlotId) => await DentistSlotDAO.Instance.getDentistSlotByID(dentistSlotId);
        public async Task CreateDentistSlot(DentistSlot dentistSlot) => await DentistSlotDAO.Instance.createDentistSlot(dentistSlot);

        public List<DentistSlot> GetAllDentistSlotByServiceAndTimeStart(int serviceId, DateTime timeStart) =>
            DentistSlotDAO.Instance.getAllDentistSlotsByServiceAndDate(serviceId, timeStart);

        public async Task DeleteDentistSlot(DentistSlot dentistSlot) => await DentistSlotDAO.Instance.deleteDentistSlot(dentistSlot);
        public async Task UpdateDentistSlot(DentistSlot dentistSlot) => await DentistSlotDAO.Instance.updateDentistSlot(dentistSlot);

    }
}
