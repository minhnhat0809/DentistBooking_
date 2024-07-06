using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class CheckupScheduleService : ICheckupScheduleService
    {
        private readonly ICheckupScheduleRepo _scheduleRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public CheckupScheduleService(ICheckupScheduleRepo scheduleRepo, IMapper mapper, IUserRepo userRepo)
        {
            _scheduleRepo = scheduleRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public async Task<List<CheckupScheduleDto>> GetAllCheckupSchedules()
        {
            try
            {
                var models = await _scheduleRepo.GetAllCheckupSchedules();
                var viewModels = _mapper.Map<List<CheckupScheduleDto>>(models);
                foreach (var viewModel in viewModels)
                {
                    var schedule = models.FirstOrDefault(x => x.ScheduleId == viewModel.ScheduleId);
                    if (schedule != null)
                    {
                        viewModel.DentistName = _userRepo.GetById(schedule.DentistId)?.Result.Name ;
                        viewModel.CustomerName = _userRepo.GetById(schedule.CustomerId)?.Result.Name ;
                    }
                }
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving checkup schedules.", ex);
            }
        }


        public async void CreateCheckupSchedule(CheckupSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule), "Schedule cannot be null.");
            }

            try
            {
                var existingSchedule = await _scheduleRepo.GetById(schedule.ScheduleId);
                if (existingSchedule != null)
                {
                    throw new InvalidOperationException($"Checkup schedule with ID {schedule.ScheduleId} already exists.");
                }

                _scheduleRepo.CreateCheckupSchedule(schedule);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the checkup schedule.", ex);
            }
        }

        public async void UpdateCheckupSchedule(CheckupSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule), "Schedule cannot be null.");
            }

            try
            {
                var existingSchedule = await _scheduleRepo.GetById(schedule.ScheduleId);
                if (existingSchedule == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Checkup schedule with ID {schedule.ScheduleId} not found.");
                }

                _scheduleRepo.UpdateCheckupSchedule(schedule);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the checkup schedule.", ex);
            }
        }

        public async void DeleteCheckupSchedule(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID.", nameof(id));
            }

            try
            {
                var existingSchedule = await _scheduleRepo.GetById(id);
                if (existingSchedule == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Checkup schedule with ID {id} not found.");
                }

                _scheduleRepo.DeleteCheckupSchedule(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the checkup schedule.", ex);
            }
        }

        public async Task<CheckupScheduleDto> GetDtoById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID.", nameof(id));
            }

            try
            {
                var model = await  _scheduleRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Checkup schedule with ID {id} not found.");
                }
                var viewModel = _mapper.Map<CheckupScheduleDto>(model); 
                    viewModel.DentistName = _userRepo.GetById(model.DentistId).Result.Name ?? "Unknown Dentist";
                    viewModel.CustomerName = _userRepo.GetById(model.CustomerId).Result.Name ?? "Unknown Customer";
                

                return viewModel;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the checkup schedule.", ex);
            }
        }
        public async Task<CheckupSchedule> GetById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID.", nameof(id));
            }

            try
            {
                var model = await _scheduleRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Checkup schedule with ID {id} not found.");
                }
                

                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the checkup schedule.", ex);
            }
        }
    }

    
}
