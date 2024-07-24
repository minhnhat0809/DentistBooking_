using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Service.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Result;
using Microsoft.IdentityModel.Tokens;

namespace Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo appointmentRepo;
        private readonly IDentistSlotRepo dentistSlotRepo;
        private readonly IServiceRepo serviceRepo;
        private readonly IUserRepo userRepo;
        private readonly IMedicalRecordRepo medicalRecordRepo;
        private readonly IDentistServiceRepo _dentistServiceRepo;
        private readonly IMapper mapper;

        public AppointmentService(IAppointmentRepo appointmentRepo, IDentistSlotRepo dentistSlotRepo, 
            IServiceRepo serviceRepo, IUserRepo userRepo, IMedicalRecordRepo medicalRecordRepo, IDentistServiceRepo _dentistServiceRepo,
            IMapper mapper)
        {
            this.appointmentRepo = appointmentRepo;
            this.dentistSlotRepo = dentistSlotRepo;
            this.serviceRepo = serviceRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
            this.medicalRecordRepo = medicalRecordRepo;
            this._dentistServiceRepo = _dentistServiceRepo;
        }
        public async Task<AppointmentResult> AddAppointment(AppointmentDto appointment, string email)
        {
            AppointmentResult appointmentResult = new AppointmentResult();
            try
            {
                if (appointment == null)
                {
                    appointmentResult.Message = "Appointment is null!";
                    return appointmentResult;
                }
            
                if (appointment.ServiceId <= 0)
                {
                    appointmentResult.Message = "Service Id is null!";
                    return appointmentResult;
                }

                BusinessObject.Service serviceDto = await serviceRepo.GetServiceByID(appointment.ServiceId.Value);
                if (serviceDto == null)
                {
                    appointmentResult.Message = "Service is not exist!";
                    return appointmentResult;
                }

                if (appointment.DentistSlotId <= 0)
                {
                    appointmentResult.Message = "Dentist slot Id is null!";
                    return appointmentResult;
                }
                
                DentistSlot dentistSlot = await dentistSlotRepo.GetDentistSlotByID(appointment.DentistSlotId.Value);
                if (dentistSlot == null)
                {
                    appointmentResult.Message = "Dentist slot is not exist!";
                    return appointmentResult;
                }
                else if(dentistSlot.Status == false)
                {
                    appointmentResult.Message = "Dentist slot is disabled!";
                    return appointmentResult;
                }

                if (dentistSlot.Dentist.Status == false)
                {
                    appointmentResult.Message = "Dentist is disabled!";
                    return appointmentResult;
                }
                
                

                if (appointment.TimeStart < dentistSlot.TimeStart || appointment.TimeEnd > dentistSlot.TimeEnd)
                {
                    appointmentResult.Message = "The time of this appointment is out of range for this dentist slot!";
                    return appointmentResult;
                }

                var validStatuses = new[] {  "Not yet arrived", "Arrived", "Happening", "Finished", "Processing" };
                if (!validStatuses.Contains(appointment.Status))
                {
                    appointmentResult.Message = "Status is not valid!";
                    return appointmentResult;
                    
                }

                if (appointment.TimeStart > appointment.TimeEnd)
                {
                    appointmentResult.Message = "Time Start is bigger than Time End!";
                    return appointmentResult;
                }

                if (!CheckTimeStart(appointment.TimeStart))
                {
                    appointmentResult.Message = "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
                    return appointmentResult;
                }

                User? user = userRepo.GetUserByUserName(email).Result;
                if (user != null)
                {
                    appointment.ModifiedBy = user.UserId;
                }

                var appoinmentList = dentistSlot.Appointments.ToList();
                appoinmentList = appoinmentList.Where(ap => !(ap.Status.Equals("Processing")  ||  ap.Status.Equals("Delete"))).ToList();
                if (appoinmentList != null)
                {
                    foreach (var ap in appoinmentList)
                    {
                            TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                            TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                            TimeSpan timeStart = appointment.TimeStart.TimeOfDay;
                            TimeSpan timeEnd = appointment.TimeStart.AddMinutes(30).TimeOfDay;

                            if ((timeStart >= apStartTime && timeStart < apEndTime) || (timeStart < apStartTime && timeEnd > apStartTime))
                            {
                                appointmentResult.Message = $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}'";
                                return appointmentResult;
                            }
                    }
                }

                
                var viewModel = mapper.Map<Appointment>(appointment);
                viewModel.CreateBy = dentistSlot.DentistId;
                
                 await appointmentRepo.CreateAppointment(viewModel);
                appointmentResult.Message = "Success";
                return appointmentResult;
            }
            catch (Exception e)
            {
                appointmentResult.Message = e.Message;
                return appointmentResult;
            }
        }
        public async Task<Dictionary<string, string>> CreateAppointment(DateTime TimeStart, int customerId, DateOnly selectedDate, int serviceId)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            void AddError(string field, string message)
            {
                if (!errors.ContainsKey(field))
                {
                    errors[field] = "";
                }
                errors[field] = message;
            }

            try
            {
                BusinessObject.Service serviceDto = await serviceRepo.GetServiceByID(serviceId);
                if (serviceDto == null)
                {
                    AddError("Service", "Service is not existed!");
                    return errors;
                }

                if (!CheckTimeStart(TimeStart))
                {
                    AddError("TimeStart", "Time must be in range [8:00-11:30] & [13:00-19:00]");
                    return errors;
                }

                List<Appointment> appointments = await appointmentRepo.GetAllAppointmentsOfCustomer(customerId);

                var appoinmentList = appointments.Where(a => DateOnly.FromDateTime(a.TimeStart) == selectedDate).ToList();
                
                if (appoinmentList != null)
                {
                    foreach (var ap in appoinmentList)
                    {
                        TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                        TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                        TimeSpan timeStart = TimeStart.TimeOfDay;
                        TimeSpan timeEnd = TimeStart.AddMinutes(30).TimeOfDay;

                        if ((timeStart >= apStartTime && timeStart < apEndTime) || (timeStart < apStartTime && timeEnd > apStartTime))
                        {
                            AddError("",$"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}'");
                            return errors;
                        }
                    }
                }

                DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month,
                    selectedDate.Day, TimeStart.Hour, TimeStart.Minute, TimeStart.Second);


                Appointment appointment = new Appointment();
                appointment.TimeStart = combinedDateTime;
                appointment.TimeEnd = TimeStart.AddMinutes(30);
                appointment.CustomerId = customerId;
                appointment.Status = "Processing";
                appointment.ServiceId = serviceId;

                await appointmentRepo.CreateAppointment(appointment);
                AddError("Success", "Create Successfully!");
                return errors;
            }
            catch (Exception e)
            {
                AddError("Error", "Error while creating appointment");
                return errors;
            }
        }
        
        public async Task<Dictionary<string, string>> CreateAppointmentWithDentist(DateTime TimeStart, int customerId, DateOnly selectedDate, int serviceId, int dentistId)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            void AddError(string field, string message)
            {
                if (!errors.ContainsKey(field))
                {
                    errors[field] = "";
                }
                errors[field] = message;
            }

            try
            {
                BusinessObject.Service serviceDto = await serviceRepo.GetServiceByID(serviceId);
                if (serviceDto == null)
                {
                    AddError("Service", "Service is not existed!");
                    return errors;
                }

                if (dentistId > 0)
                {
                    User? dentist = await userRepo.GetById(dentistId);
                    if (dentist == null)
                    {
                        AddError("Dentist", "Dentist is not existed!");
                        return errors;
                    } 
                    
                    if (dentist.Status == false)
                    {
                        AddError("Dentist", "Dentist is disabled!");
                        return errors;
                    }

                    List<BusinessObject.Service> services = await _dentistServiceRepo.GetAllServiceByDentistActive(dentistId);
                    if (!services.Any(s => s.ServiceId == serviceId))
                    {
                        AddError("Service", "Dentist does not do this service!");
                        return errors;
                    }
                }

                if (customerId <= 0)
                {
                    AddError("Customer", "Customer Id is null!");
                    return errors;
                }
                else
                {
                    User? customer = await userRepo.GetById(customerId);
                    if (customer == null)
                    {
                        AddError("Customer", "Customer is not existed!");
                        return errors;
                    }
                    else if(customer.Status == false)
                    {
                        AddError("Customer", "Customer is disable!");
                        return errors;
                    }
                }
                

                if (!CheckTimeStart(TimeStart))
                {
                    AddError("TimeStart", "Time must be in range [8:00-11:30] & [13:00-19:00]");
                    return errors;
                }

                List<Appointment> appointments = await appointmentRepo.GetAllAppointmentsOfCustomer(customerId);

                var appoinmentList = appointments.Where(a => DateOnly.FromDateTime(a.TimeStart) == selectedDate).ToList();
                
                if (appoinmentList != null)
                {
                    foreach (var ap in appoinmentList)
                    {
                        TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                        TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                        TimeSpan timeStart = TimeStart.TimeOfDay;
                        TimeSpan timeEnd = TimeStart.AddMinutes(30).TimeOfDay;

                        if ((timeStart >= apStartTime && timeStart < apEndTime) || (timeStart < apStartTime && timeEnd > apStartTime))
                        {
                            AddError("",$"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}'");
                            return errors;
                        }
                    }
                }

                DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month,
                    selectedDate.Day, TimeStart.Hour, TimeStart.Minute, TimeStart.Second);


                Appointment appointment = new Appointment();
                appointment.TimeStart = combinedDateTime;
                appointment.TimeEnd = TimeStart.AddMinutes(30);
                appointment.CustomerId = customerId;
                appointment.Status = "Processing";
                appointment.ServiceId = serviceId;

                if (dentistId > 0)
                {
                    appointment.CreateBy = dentistId;
                }

                await appointmentRepo.CreateAppointment(appointment);
                AddError("Success", "Create Successfully!");
                return errors;
            }
            catch (Exception e)
            {
                AddError("Error", "Error while creating appointment");
                return errors;
            }
        }

        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            List<Appointment> appointmentList = new List<Appointment>();

            appointmentList = await appointmentRepo.GetAllAppointments();

            var viewModels = mapper.Map<List<AppointmentDto>>(appointmentList);  
            return viewModels;
        }

        public async Task<List<AppointmentDto>> GetALlAppointmentsOfCustomer(int customerId)
        {
            List<Appointment> appointmentList = new List<Appointment>();

            appointmentList = await appointmentRepo.GetAllAppointmentsOfCustomer(customerId);
            var viewModels = mapper.Map<List<AppointmentDto>>(appointmentList);
            return viewModels;
        }

        public async Task<AppointmentDto> GetAppointmentByID(int appointmentId)
        {
            Appointment appointment = await appointmentRepo.GetAppointmentById(appointmentId);
            var viewModel = mapper.Map<AppointmentDto>(appointment);
            return viewModel;
        }
        public async Task<Dictionary<string, string>> UpdateAppointment(int serviceId, int appointmentId, DateTime TimeStart, int customerId, int dentistId)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            void AddError(string field, string message)
            {
                errors[field] = message;
            }
            try
            { 
                if (serviceId <= 0)
                {
                    AddError("Service", "Service Id is empty!");
                    return errors;
                }

                BusinessObject.Service serviceDto = await serviceRepo.GetServiceByID(serviceId);
                if (serviceDto == null)
                {
                    AddError("Service", "Service is not existed!");
                    return errors;
                }
                
                if (dentistId > 0)
                {
                    User? dentist = await userRepo.GetById(dentistId);
                    if (dentist == null)
                    {
                        AddError("Dentist", "Dentist is not existed!");
                        return errors;
                    }else if (dentist.Status == false)
                    {
                        AddError("Dentist", "Dentist is disabled!");
                        return errors;
                    }

                    List<BusinessObject.Service> services = await _dentistServiceRepo.GetAllServiceByDentistActive(dentistId);
                    if (!services.Any(s => s.ServiceId == serviceId))
                    {
                        AddError("Service", "Dentist does not do this service!");
                        return errors;
                    }
                }

                if (customerId <= 0)
                {
                    AddError("Customer", "Customer Id is empty!");
                    return errors;
                }

                User cusomter = userRepo.GetById(customerId).Result;
                if (cusomter == null)
                {
                    AddError("Customer", "Customer is not existed!");
                    return errors;
                }

                if (!CheckTimeStart(TimeStart))
                {
                    AddError("TimeStart", "Time must be in range [8:00-11:30] & [13:00-19:00]");
                    return errors;
                }

                Appointment appointment = await appointmentRepo.GetAppointmentById(appointmentId);
                if (appointment == null)
                {
                    AddError("Appointment", "Appointment is not existed!");
                    return errors;
                }

                switch (appointment.Status)
                {
                    case "Arrived":
                        AddError("Status", "Cannot update! This appointment is about to happening!");
                        return errors;
                    case "Happening":
                        AddError("Status", "Cannot update! This appointment is happening!");
                        return errors;
                    case "Finished":
                        AddError("Status", "Cannot update! This appointment is finished!");
                        return errors;
                    case "Deleted":
                        AddError("Status", "Cannot update! This appointment is deleted!");
                        return errors;
                }

                if (!appointment.TimeStart.Equals(TimeStart))
                {
                    appointment.DentistSlotId = null;
                    appointment.TimeStart = TimeStart;
                    appointment.TimeEnd = TimeStart.AddMinutes(30);
                }

                appointment.ServiceId = serviceId;
                if (dentistId > 0)
                {
                    appointment.CreateBy = dentistId;
                }
                else
                {
                    appointment.CreateBy = null;
                }
                appointment.Status = "Processing";
                

                appointmentRepo.UpdateAppointment(appointment);
                AddError("Success", "Update successfully!");
                return errors;
            }
            catch (Exception e)
            {
                AddError("Error", "Error while update appointment!");
                return errors;
            }
        }
        private bool CheckTimeStart(DateTime TimeStart)
        {

            TimeSpan morningStart = new TimeSpan(8, 0, 0);
            TimeSpan morningEnd = new TimeSpan(11, 30, 0);
            TimeSpan afternoonStart = new TimeSpan(13, 0, 0);
            TimeSpan afternoonEnd = new TimeSpan(19, 0, 0);


            TimeSpan timeOfDay = TimeStart.TimeOfDay;


            bool isInMorning = timeOfDay >= morningStart && timeOfDay <= morningEnd;
            bool isInAfternoon = timeOfDay >= afternoonStart && timeOfDay <= afternoonEnd;

            return isInMorning || isInAfternoon;
        }

        public async Task<List<AppointmentDto>> GetAllProcessingAppointment()
        {
            var models = await appointmentRepo.GetAllProcessingAppointment();
            var viewModels = mapper.Map<List<AppointmentDto>>(models);  
            return viewModels;
        }

       public async Task<AppointmentResult> UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId)
        {
            AppointmentResult appointmentResult = new AppointmentResult();
        try
        {
            if (serviceId <= 0)
            {
                appointmentResult.Message = "Service ID is empty!";
                return appointmentResult;
            }

            BusinessObject.Service serviceModel = await serviceRepo.GetServiceByID(serviceId);
            if (serviceModel == null)
            {
                appointmentResult.Message = "This service does not exist!";
                return appointmentResult;
            }

            if (appointmentId <= 0)
            {
                appointmentResult.Message = "Appointment ID is empty!";
                return appointmentResult;
            }
            Appointment appointment = await appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                appointmentResult.Message = "This appointment does not exist!";
                return appointmentResult;
            }

            if (dentistSlotId <= 0)
            {
                appointmentResult.Message = "Dentist slot ID is empty!";
                return appointmentResult;
            }
            DentistSlot dentistSlot = await dentistSlotRepo.GetDentistSlotByID(dentistSlotId);
            if (dentistSlot == null)
            {
                appointmentResult.Message = "This dentist slot does not exist!";
                return appointmentResult;
            }else if(dentistSlot.Status == false)
            {
                appointmentResult.Message = "Dentist slot is disabled!";
                return appointmentResult;
            }

            if (TimeStart < dentistSlot.TimeStart || TimeEnd > dentistSlot.TimeEnd)
            {
                appointmentResult.Message = "The time of this appointment is out of range for this dentist slot!";
                return appointmentResult;
            }

            if (TimeStart > TimeEnd)
            {
                appointmentResult.Message = "Time Start is greater than Time End!";
                return appointmentResult;
            }

            if (!CheckTimeStart(TimeStart))
            {
                appointmentResult.Message = "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
                return appointmentResult;
            }

            var appointmentList = dentistSlot.Appointments.ToList();
            appointmentList = appointmentList.Where(ap => !(ap.Status.Equals("Delete"))).ToList();
            if (appointmentList != null)
            {
                foreach (var ap in appointmentList)
                {
                    if (ap.AppointmentId != appointment.AppointmentId)
                    {
                        TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                        TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                        TimeSpan timeStart = TimeStart.TimeOfDay;
                        TimeSpan timeEnd = TimeEnd.TimeOfDay;

                        if ((timeStart >= apStartTime && timeStart < apEndTime) || (timeStart < apStartTime && timeEnd > apStartTime))
                        {
                            appointmentResult.Message = $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}";
                            return appointmentResult;
                        }
                    }
                    
                }
            }

            appointment.ServiceId = serviceId;
            appointment.Status = "Not yet arrived";
            appointment.DentistSlotId = dentistSlotId;
            appointment.TimeStart = TimeStart;
            appointment.TimeEnd = TimeEnd;

            await appointmentRepo.UpdateAppointment(appointment);

            AppointmentDto appointmentDto = mapper.Map<AppointmentDto>(appointment);
            
            appointmentResult.Message = "Success";
            appointmentResult.Appointment = appointmentDto;
            return appointmentResult;
        }
        catch (Exception e)
        {
            appointmentResult.Message = e.Message;
            return appointmentResult;
        }
        }
       
        public async Task PutAppointment(AppointmentDto appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "appointment cannot be null.");
            }

            try
            {
                var model = await appointmentRepo.GetAppointmentById(appointment.AppointmentId);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Appointment with ID {appointment.AppointmentId} not found.");
                }
                model = mapper.Map<Appointment>(appointment);  
                await appointmentRepo.UpdateAppointment(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating.", ex);
            }
        }

        public async Task<string> DeleteAppointment(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return "Invalid appointment ID.";
            }

            try
            {
                var existing = await appointmentRepo.GetAppointmentById(appointmentId);
                if (existing == null)
                {
                    return $"Appointment with ID {appointmentId} not found.";
                }
                await appointmentRepo.DeleteAppointment(appointmentId);
                return "Success";
            }
            catch (Exception ex)
            {
                
                return "An error occurred while deleting the checkup schedule.";
            }
        }
        public async Task<AppointmentResult> UpdateAppointments(AppointmentDto appointMent, string email)
        {
            AppointmentResult appointmentResult = new AppointmentResult();
            try
            {
                if (appointMent.ServiceId <= 0)
                {
                    appointmentResult.Message = "Service ID is empty!";
                    return appointmentResult;
                }

                BusinessObject.Service serviceDto = await serviceRepo.GetServiceByID(appointMent.ServiceId);
                if (serviceDto == null)
                {
                    appointmentResult.Message = "This service does not exist!";
                    return appointmentResult;
                }

                if (appointMent.AppointmentId <= 0)
                {
                    appointmentResult.Message = "Appointment ID is empty!";
                    return appointmentResult;
                }
                Appointment appointment = await appointmentRepo.GetAppointmentById(appointMent.AppointmentId);
                if (appointment == null)
                {
                    appointmentResult.Message = "This appointment does not exist!";
                    return appointmentResult;
                }

                if (appointMent.DentistSlotId <= 0)
                {
                    appointmentResult.Message = "Dentist slot ID is empty!";
                    return appointmentResult;
                }
                DentistSlot dentistSlot = await dentistSlotRepo.GetDentistSlotByID(appointMent.DentistSlotId.Value);
                if (dentistSlot == null)
                {
                    appointmentResult.Message = "This dentist slot does not exist!";
                    return appointmentResult;
                }else if(dentistSlot.Status == false)
                {
                    appointmentResult.Message = "Dentist slot is disabled!";
                    return appointmentResult;
                }

                if (dentistSlot.Dentist.Status == false)
                {
                    appointmentResult.Message = "This dentist is disabled!";
                    return appointmentResult;
                }

                if (appointMent.TimeStart < dentistSlot.TimeStart || appointMent.TimeEnd > dentistSlot.TimeEnd)
                {
                    appointmentResult.Message = "The time of this appointment is out of range for this dentist slot!";
                    return appointmentResult;
                }

                var validStatuses = new[] { "Not yet arrived", "Arrived", "Happening", "Finished", "Processing" };
                if (!validStatuses.Contains(appointMent.Status))
                {
                    appointmentResult.Message = "Status is not valid!";
                    return appointmentResult;
                }

                if (appointMent.TimeStart > appointMent.TimeEnd)
                {
                    appointmentResult.Message = "Time Start is greater than Time End!";
                    return appointmentResult;
                }

                if (!CheckTimeStart(appointMent.TimeStart))
                {
                    appointmentResult.Message = "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
                    return appointmentResult;
                }

                var appointmentList = dentistSlot.Appointments.ToList();
                appointmentList = appointmentList.Where(ap => !(ap.Status.Equals("Delete"))).ToList();
                if (appointmentList != null)
                {
                    foreach (var ap in appointmentList)
                    {
                        if (ap.AppointmentId != appointment.AppointmentId)
                        {
                            TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                            TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                            TimeSpan timeStart =appointMent.TimeStart.TimeOfDay;
                            TimeSpan timeEnd = appointMent.TimeEnd.TimeOfDay;

                            if ((timeStart >= apStartTime && timeStart < apEndTime) || (timeStart < apStartTime && timeEnd > apStartTime))
                            {
                                appointmentResult.Message = $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}";
                                return appointmentResult;
                            }
                        }
                    }
                }

                if (email.IsNullOrEmpty())
                {
                    appointmentResult.Message = "Email of modified user is null!";
                    return appointmentResult; 
                }

                User? user = await userRepo.GetUserByUserName(email);
                if (user == null)
                {
                    appointmentResult.Message = "Modified user is not exist!";
                    return appointmentResult; 
                }

                if (appointMent.MedicalRecordId > 0 )
                {
                    MedicalRecord medicalRecord = medicalRecordRepo.GetById(appointMent.MedicalRecordId).Result;
                    if (medicalRecord == null)
                    {
                        appointmentResult.Message = "Medical record is not found!";
                        return appointmentResult;
                    }
                    else
                    {
                        appointment.MedicalRecordId = appointMent.MedicalRecordId;
                    }
                }

                appointment.ServiceId = appointMent.ServiceId;
                appointment.Status = appointMent.Status;
                appointment.DentistSlotId = appointMent.DentistSlotId;
                appointment.TimeStart = appointMent.TimeStart;
                appointment.TimeEnd = appointMent.TimeEnd;
                appointment.MedicalRecordId = appointment.MedicalRecordId;
                appointment.ModifiedBy = user.UserId;
                appointment.Diagnosis = appointMent.Diagnosis;

                await appointmentRepo.UpdateAppointment(appointment);
                AppointmentDto appointmentDto = mapper.Map<AppointmentDto>(appointment);
                
                appointmentResult.Message = "Success";
                appointmentResult.Appointment = appointmentDto;
                return appointmentResult;
            }
            catch (Exception e)
            {
                appointmentResult.Message = e.Message;
                return appointmentResult;
            }
        }


        public async Task<List<string>> GetAllStatusOfAppointment(int appointmentId)
        {
            List<string> statusList = new List<string> { "Not yet arrived", "Arrived", "Happening", "Finished"};
            if (appointmentId == 0)
            {
                return statusList;
            }
            Appointment appointment = await appointmentRepo.GetAppointmentById(appointmentId);

            var s = statusList.Where(s => s.Equals(appointment.Status)).FirstOrDefault();
            statusList.Remove(s);
            statusList.Insert(0, s);
            
            return statusList;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentByDentistId(int dentistId)
        {
            
            var models = await appointmentRepo.GetAllAppointmentsByDentist(dentistId);
            models = models.Where(x => x.DentistSlot.DentistId == dentistId).ToList();
            var viewModels = mapper.Map<List<AppointmentDto>>(models);  
            return viewModels;
        }

        public async Task<AppointmentResult> DeleteAppointmentForStaff(int appointmentId, string customerName, string reason)
        {
            AppointmentResult appointmentResult = new AppointmentResult();
            try
            {
                Appointment? appointment = await appointmentRepo.GetAppointmentById(appointmentId);
                if (!appointment.Customer.Name.Equals(customerName))
                {
                    appointmentResult.Message = "Wrong customer name!";
                    return appointmentResult;
                }

                appointmentRepo.DeleteAppointment(appointmentId);
                appointmentResult.Message = "Success";
                return appointmentResult;
            }
            catch (Exception e)
            {
                appointmentResult.Message = e.Message;
                return appointmentResult;
            }
        }
    }
}
