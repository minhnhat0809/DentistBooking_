﻿using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Result;
using Microsoft.IdentityModel.Tokens;

namespace Service.Impl
{
    public class DentistSlotService : IDentistSlotService
    {
        private readonly IDentistSlotRepo dentistSlotRepo;
        private readonly IUserRepo userRepo;
        private readonly IMapper mapper;
        private readonly IRoomRepo _roomRepo;
        private readonly IServiceRepo _serviceRepo;

        public DentistSlotService(IDentistSlotRepo dentistSlotRepo, IUserRepo userRepo, IMapper mapper, IRoomRepo roomRepo, IServiceRepo serviceRepo)
        {
            this.dentistSlotRepo = dentistSlotRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
            _roomRepo = roomRepo;
            _serviceRepo = serviceRepo;
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

        public async Task<DentistSlotResult> CreateDentistSlot(int dentistId, DateTime timeStart, DateTime timeEnd, int RoomId)
        {
            DentistSlotResult dentistSlotResult = new DentistSlotResult();
            try
            {
                if (dentistId <= 0)
                {
                    dentistSlotResult.Message = "Dentist Id is null!";
                    return dentistSlotResult;
                }

                User dentist = await userRepo.GetById(dentistId);
                if (dentist == null)
                {
                    dentistSlotResult.Message = "Dentist is not exist!";
                    return dentistSlotResult;
                }

                if (RoomId <= 0)
                {
                    dentistSlotResult.Message = "Room Id is null!";
                    return dentistSlotResult;
                }

                Room? room = _roomRepo.GetRoomById(RoomId);
                if (room == null)
                {
                    dentistSlotResult.Message = "Room is not exist!";
                    return dentistSlotResult;
                }

                if (!timeStart.Date.Equals(timeEnd.Date))
                {
                    dentistSlotResult.Message = "Date of time start and time end is different!";
                    return dentistSlotResult;
                } else if (timeStart.TimeOfDay > timeEnd.TimeOfDay)
                {
                    dentistSlotResult.Message = "Time start is bigger than time end!";
                    return dentistSlotResult;
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
                    dentistSlotResult.Message = "Time must be in range [8:00-12:00] , [13:00-17:00], [17:00-19:30]!";
                    return dentistSlotResult;
                }
                
                List<DentistSlot> dentistSlots = await dentistSlotRepo.GetAllDentistSlotsByDentistAndDate(dentistId, DateOnly.FromDateTime(timeStart));
                if (dentistSlots.Count > 0)
                {
                    if (dentistSlots.Any(dl => dl.TimeStart == timeStart))
                    {
                        dentistSlotResult.Message = "There is a slot with this time range!";
                        return dentistSlotResult;
                    }
                }

                List<DentistSlot> dentistSLots = await dentistSlotRepo.GetAllDentistSlotsByRoomAndDate(RoomId, timeStart);
                if (dentistSLots.Count > 0)
                {
                    dentistSlotResult.Message = "There is a dentist using this room in this range time";
                    return dentistSlotResult;
                }

                DentistSlot dentistSlot = new DentistSlot();
                dentistSlot.DentistId = dentistId;
                dentistSlot.TimeStart = timeStart;
                dentistSlot.TimeEnd = timeEnd;
                dentistSlot.Status = true;
                dentistSlot.RoomId = RoomId;
            
                await dentistSlotRepo.CreateDentistSlot(dentistSlot);
                dentistSlotResult.Message = "Success";
                return dentistSlotResult;
            }
            catch (Exception e)
            {
                dentistSlotResult.Message = e.Message;
                return dentistSlotResult;
            }
        }

        public ListDentistSlotResult GetDentistSlotByServiceAndDateTime(int serviceId, DateTime timeStart)
        {
            ListDentistSlotResult listDentistSlotResult = new ListDentistSlotResult();
            try
            {
                if (serviceId <= 0)
                {
                    listDentistSlotResult.Message = "Service Id is null!";
                    return listDentistSlotResult;
                }

                BusinessObject.Service service = _serviceRepo.GetServiceByID(serviceId).Result;
                if (service == null)
                {
                    listDentistSlotResult.Message = "This service is not exist!";
                    return listDentistSlotResult;
                }
                
                /*DateTime now = DateTime.Now;
                if (timeStart.Date < now.Date)
                {
                    listDentistSlotResult.Message = "Selected date is smaller than today!";
                    return listDentistSlotResult;
                }*/
                
                List<DentistSlot> dentistSlots =  dentistSlotRepo.GetAllDentistSlotByServiceAndTimeStart(serviceId, timeStart);
                listDentistSlotResult.DentistSlots = dentistSlots;
                listDentistSlotResult.Message = "Success";
                return listDentistSlotResult;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*public ListDentistSlotResult GetDentistSlotByServiceAndDate(int serviceId, DateTime timeStart)
        {
            ListDentistSlotResult listDentistSlotResult = new ListDentistSlotResult();
            try
            {
                if (serviceId <= 0)
                {
                    listDentistSlotResult.Message = "Service Id is null!";
                    return listDentistSlotResult;
                }

                BusinessObject.Service service = _serviceRepo.GetServiceByID(serviceId).Result;
                if (service == null)
                {
                    listDentistSlotResult.Message = "This service is not exist!";
                    return listDentistSlotResult;
                }
                
                /*DateTime now = DateTime.Now;
                if (timeStart.Date < now.Date)
                {
                    listDentistSlotResult.Message = "Selected date is smaller than today!";
                    return listDentistSlotResult;
                }#1#
                
                List<DentistSlot> dentistSlots =  dentistSlotRepo.GetAllDentistSlotByServiceAndDate(serviceId, timeStart);
                listDentistSlotResult.DentistSlots = dentistSlots;
                listDentistSlotResult.Message = "Success";
                return listDentistSlotResult;
            }
            catch (Exception e)
            {
                listDentistSlotResult.Message = e.Message;
                return listDentistSlotResult;
            }
        }*/

        public ListDentistSlotResult GetDentistSlotForAppointment(List<DentistSlot> dentistSlots, int dentistSlotId)
        {
            ListDentistSlotResult listDentistSlotResult = new ListDentistSlotResult();
            try
            {
                if (!dentistSlots.IsNullOrEmpty())
                {
                    var s = dentistSlots.FirstOrDefault(dl => dl.DentistSlotId == dentistSlotId);
                    if (s != null)
                    {
                        dentistSlots.Remove(s);
                        dentistSlots.Insert(0, s);
                    }

                    listDentistSlotResult.DentistSlots = dentistSlots;
                }
                

                listDentistSlotResult.Message = "Success";
            }
            catch (Exception e)
            {
                listDentistSlotResult.Message = e.Message;
            }

            return listDentistSlotResult;
        }

        public DentistSlotResult GetDentistSlotByAppointmentTimeStart(DateTime TimeStart, int dentistId)
        {
            DentistSlotResult dentistSlotResult = new DentistSlotResult();
            try
            {
                DentistSlot dentistSlot =
                     dentistSlotRepo.GetDentistSlotByDentistAndTimeStart(dentistId, TimeStart);
                
                dentistSlotResult.Message = "Success";
                dentistSlotResult.DentistSlot = mapper.Map<DentistSlotDto>(dentistSlot);
            }
            catch (Exception e)
            {
                dentistSlotResult.Message = e.Message;
            }

            return dentistSlotResult;
        }

        public async Task<DentistSlotResult> UpdateStatusDentistSlot(bool status, int dentistSlotId)
        {
            DentistSlotResult dentistSlotResult = new DentistSlotResult();
            try
            {
                DentistSlot dentistSlot = await dentistSlotRepo.GetDentistSlotByID(dentistSlotId);
                if (dentistSlot == null)
                {
                    dentistSlotResult.Message = "This dentist slot is not exist!";
                    return dentistSlotResult;
                }

                dentistSlot.Status = status;

                await dentistSlotRepo.UpdateDentistSlot(dentistSlot);
                dentistSlotResult.Message = "Success";
            }
            catch (Exception e)
            {
                dentistSlotResult.Message = e.Message;
            }
            return dentistSlotResult;
        }
    }
}
