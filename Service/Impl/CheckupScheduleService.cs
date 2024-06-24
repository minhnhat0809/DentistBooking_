using System;
using System.Collections.Generic;
using BusinessObject;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class CheckupScheduleService : ICheckupScheduleService
    {
        private readonly ICheckupScheduleRepo _scheduleRepo;

        public CheckupScheduleService(ICheckupScheduleRepo scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public List<CheckupSchedule> GetAllCheckupSchedules()
        {
            try
            {
                return _scheduleRepo.GetAllCheckupSchedules();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving checkup schedules.", ex);
            }
        }

        public CheckupSchedule GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID.", nameof(id));
            }

            try
            {
                var model = _scheduleRepo.GetById(id);
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

        public void CreateCheckupSchedule(CheckupSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule), "Schedule cannot be null.");
            }

            try
            {
                var existingSchedule = _scheduleRepo.GetById(schedule.ScheduleId);
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

        public void UpdateCheckupSchedule(CheckupSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule), "Schedule cannot be null.");
            }

            try
            {
                var existingSchedule = _scheduleRepo.GetById(schedule.ScheduleId);
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

        public void DeleteCheckupSchedule(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID.", nameof(id));
            }

            try
            {
                var existingSchedule = _scheduleRepo.GetById(id);
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
    }

    
}
