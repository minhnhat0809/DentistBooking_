using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
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
        private readonly IMapper mapper;

        public DentistSlotService(IDentistSlotRepo dentistSlotRepo, IUserRepo userRepo, IMapper mapper)
        {
            this.dentistSlotRepo = dentistSlotRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
        }
        public async Task<List<DentistSlotDto>> GetAllDentistSlots()
        {
            try
            {
                var models = await dentistSlotRepo.GetAllDentistSlots();
                if(models == null) 
                {
                    throw new Exception("dentist slot not found");
                }
                var viewModels = mapper.Map<List<DentistSlotDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get all dentist slot", ex);
            }
        }

        public async Task<List<DentistSlotDto>> GetAllDentistSlotsByDentistAndDate(int id, DateOnly selectedDate)
        {
            try
            {
                var models = await dentistSlotRepo.GetAllDentistSlotsByDentistAndDate(id, selectedDate);
                if (models != null)
                {
                    throw new Exception("dentist slot not found");
                }
                var viewModels = mapper.Map<List<DentistSlotDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get all dentist slot", ex);
            }
        }

        public async  Task<DentistSlotDto> GetDentistSlotById(int dentistSlotId)
        {

            try
            {
                var model = await dentistSlotRepo.GetDentistSlotByID(dentistSlotId);
                if (model == null)
                {
                    throw new Exception("dentist slot not found");
                }
                var viewModel = mapper.Map<DentistSlotDto>(model);
                return viewModel;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get all dentist slot", ex);
            }
        }

        public async Task<string> CreateDentistSlot(int dentistId, DateTime timeStart, DateTime timeEnd)
        {
            if (dentistId <= 0)
            {
                return "Dentist Id is null!";
            }

            User dentist = await userRepo.GetById(dentistId);
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
    }
}
