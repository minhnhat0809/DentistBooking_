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
        private readonly IUserRepo userRepo;

        public DentistSlotService(IDentistSlotRepo dentistSlotRepo, IUserRepo userRepo)
        {
            this.dentistSlotRepo = dentistSlotRepo;
            this.userRepo = userRepo;
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

        public string CreateDentistSlot(int dentistId, DateTime timeStart, DateTime timeEnd)
        {
            if (dentistId <= 0)
            {
                return "Dentist Id is null!";
            }

            User dentist = userRepo.GetById(dentistId);
            if (dentist == null)
            {
                return "Dentist is not exist!";
            }

            if (timeStart > timeEnd)
            {
                return "Time start is bigger than time end!";
            }
            
            TimeSpan startTime = timeStart.TimeOfDay;
            TimeSpan endTime = timeEnd.TimeOfDay;

            // Define allowed time ranges
            var allowedRanges = new List<(TimeSpan Start, TimeSpan End)>
            {
                (new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)),   // 08:00 - 12:00
                (new TimeSpan(13, 0, 0), new TimeSpan(17, 0, 0)),  // 13:00 - 17:00
                (new TimeSpan(17, 0, 0), new TimeSpan(19, 30, 0))  // 17:00 - 19:30
            };
            
            bool isValidRange = allowedRanges.Any(range => startTime == range.Start && endTime == range.End);

            if (!isValidRange)
            {
                return "Time must be in range [8:00-12:00] , [13:00-17:00], [17:00-19:30]!";
            }

            List<DentistSlot> dentistSlots = dentistSlotRepo.GetAllDentistSlotsByDentistAndDate(dentistId, DateOnly.FromDateTime(timeStart)).Result;
            if (dentistSlots.Count > 0)
            {
                if (dentistSlots.Any(dl => dl.TimeStart == timeStart))
                {
                    return "There is a slot with this time range!";
                }
            }

            DentistSlot dentistSlot = new DentistSlot();
            dentistSlot.DentistId = dentistId;
            dentistSlot.TimeStart = timeStart;
            dentistSlot.TimeEnd = timeEnd;
            dentistSlot.Status = true;
            
            dentistSlotRepo.CreateDentistSlot(dentistSlot);
            return "Success";
        }

        public string DeleteDentistSlot(int dentistSlotId)
        {
            if (dentistSlotId <= 0)
            {
                return "Dentist slot ID is empty!";
            }

            DentistSlot dentistSlot = dentistSlotRepo.GetDentistSlotByID(dentistSlotId).Result;
            if (dentistSlot == null)
            {
                return "This dentist slot is not exist!";
            }
            
            dentistSlotRepo.DeleteDentistSlot(dentistSlot);
            return "Success";
        }
    }
}
