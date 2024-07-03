using BusinessObject;
using Repository;
using Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class DentistSlotService : IDentistSlotService
    {
        private readonly IDentistSlotRepo dentistSlotRepo;

        public DentistSlotService(IDentistSlotRepo dentistSlotRepo)
        {
            this.dentistSlotRepo = dentistSlotRepo;
        }
        public async Task<List<DentistSlot>> GetAllDentistSlots()
        {
            List<DentistSlot> dentistServiceList = await dentistSlotRepo.GetAllDentistSlots();
            return dentistServiceList;
        }

        public async Task<List<DentistSlot>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate)
        {
            List<DentistSlot> dentistSlots = await dentistSlotRepo.GetAllDentistSlotsByDentistAndDate(id, selectedDate);
            return dentistSlots;
        }

        public async Task<DentistSlot> GetDentistSlotById(int dentistSlotId)
        {
            return await dentistSlotRepo.GetDentistSlotByID(dentistSlotId);
        }
    }
}
