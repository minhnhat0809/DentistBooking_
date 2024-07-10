using BusinessObject;
using Repository;
using Service.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo appointmentRepo;
        private readonly IDentistSlotService dentistSlotService;
        private readonly IService service;
        private readonly IUserRepo userRepo;

        public AppointmentService(IAppointmentRepo appointmentRepo, IDentistSlotService dentistSlotService, IService service, IUserRepo userRepo)
        {
            this.appointmentRepo = appointmentRepo;
            this.dentistSlotService = dentistSlotService;
            this.service = service;
            this.userRepo = userRepo;
        }
        public string AddAppointment(Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                 throw new ArgumentNullException(nameof(appointment), "Schedule cannot be null.");
                }
            
                if (appointment.ServiceId <= 0)
                {
                    return "Service ID is empty!";
                }

                BusinessObject.Service sErvice = service.GetServiceByID(appointment.ServiceId.Value);
                if (sErvice == null)
                {
                    return "This service is not existed!";
                }

                if (appointment.DentistSlotId <= 0)
                {
                    return "Dentist slot ID is empty!";
                }
                DentistSlot dentistSlot = dentistSlotService.GetDentistSlotById(appointment.DentistSlotId.Value).Result;
                if (dentistSlot == null)
                {
                    return "This dentist slot is not existed!";
                }

                if (appointment.TimeStart < dentistSlot.TimeStart || appointment.TimeEnd > dentistSlot.TimeEnd)
                {
                    return "The time of this appointment is out of range for this dentist slot!";
                }

                var validStatuses = new[] { "Success", "Done", "Finished", "Processing", "Delete" };
                if (!validStatuses.Contains(appointment.Status))
                {
                    return "Status is not valid!";
                }

                if (appointment.TimeStart > appointment.TimeEnd)
                {
                    return "Time Start is bigger than Time End!";
                }

                if (!CheckTimeStart(appointment.TimeStart))
                {
                    return "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
                }

                var appoinmentList = dentistSlot.Appointments.ToList();
                if (appoinmentList != null)
                {
                    foreach (var ap in appoinmentList)
                    {
                        if (!appointment.TimeStart.Equals(appointment.TimeStart))
                        {
                            TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                            TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                            if (IsOverlap(appointment.TimeStart.TimeOfDay, apStartTime, apEndTime))
                            {
                                return $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}";
                            }

                            if (IsOverlap(appointment.TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                            {
                                return $"There is an appointment overlapping at " + $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay).ToString()}. Your appoinment needs 30'";
                            }
                        }
                    }
                }
                
                appointmentRepo.CreateAppointment(appointment);
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
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
                BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
                if (sErvice == null)
                {
                    AddError("Service", "Service is not existed!");
                    return errors;
                }

                if (!CheckTimeStart(TimeStart))
                {
                    AddError("TimeStart", "Time must be in range [8:00-11:30] & [13:00-19:00]");
                    return errors;
                }

                List<Appointment> appointments = appointmentRepo.GetAllAppointmentsOfCustomer(customerId).Result;

                var appoinmentList = appointments.Where(a => DateOnly.FromDateTime(a.TimeStart) == selectedDate).ToList();
                
                if (appoinmentList != null)
                {
                    foreach (var ap in appoinmentList)
                    {
                        TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                        TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                        if (IsOverlap(TimeStart.TimeOfDay, apStartTime, apEndTime))
                        {
                            AddError("",$"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}'");
                            return errors;
                        }

                        if (IsOverlap(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                        {
                            AddError("",$"There is an appointment overlapping at " + $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay).ToString()}. Your appoinment needs 30'");
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

        public async Task<List<Appointment>> GetAllAppointments()
        {
            List<Appointment> appointmentList = new List<Appointment>();

            appointmentList = await appointmentRepo.GetAllAppointments();

            return appointmentList;
        }

        public async Task<List<Appointment>> GetALlAppointmentsOfCustomer(int customerId)
        {
            List<Appointment> appointmentList = new List<Appointment>();

            appointmentList = await appointmentRepo.GetAllAppointmentsOfCustomer(customerId);
            return appointmentList;
        }

        public Appointment GetAppointmentByID(int appointmentId)
        {
            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);
            return appointment;
        }
        public Dictionary<string, string> UpdateAppointment(int serviceId, int appointmentId, DateTime TimeStart, int customerId)
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

            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
            if (sErvice == null)
            {
                AddError("Service", "Service is not existed!");
                return errors;
            }

            if (customerId <= 0)
            {
                AddError("Customer", "Cusotmer Id is empty!");
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

            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                AddError("Appointment", "Appointment is not existed!");
                return errors;
            }

            switch (appointment.Status)
            {
                case "Deleted":
                    AddError("Status", "Cannot update! This appointment is deleted!");
                    return errors;
                case "Happening":
                    AddError("Status", "Cannot update! This appointment is happening!");
                    return errors;
                case "Expired":
                    AddError("Status", "Cannot update! This appointment is expired!");
                    return errors;
                case "Finished":
                    AddError("Status", "Cannot update! This appointment is finished!");
                    return errors;
                    case "Done":
                    AddError("Status", "Cannot update! This appointment is done!");
                    return errors;
                default:
                    break;
            }

            appointment.ServiceId = serviceId;
            appointment.TimeStart = TimeStart;
            appointment.TimeEnd = TimeStart.AddMinutes(30);
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

        private bool IsOverlap(TimeSpan targetStart, TimeSpan apStart, TimeSpan apEnd)
        {

            return (targetStart >= apStart && targetStart < apEnd);
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

        public List<Appointment> GetAllProcessingAppointment()
        {
            return appointmentRepo.GetAllProcessingAppointment();
        }

        public string UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId)
        {
            try
            {
                if (serviceId <= 0)
            {
                return "Service ID is empty!";
            }

            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
            if (sErvice == null)
            {
                return "This service is not existed!";
            }

            if (appointmentId <= 0)
            {
                return "Appointment ID is empty!";
            }
            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return "This appointment is not existed!";
            }

            if (dentistSlotId <= 0)
            {
                return "Dentist slot ID is empty!";
            }
            DentistSlot dentistSlot = dentistSlotService.GetDentistSlotById(dentistSlotId).Result;
            if (dentistSlot == null)
            {
                return "This dentist slot is not existed!";
            }

            if (TimeStart < dentistSlot.TimeStart || TimeEnd > dentistSlot.TimeEnd)
            {
                return "The time of this appointment is out of range for this dentist slot!";
            }
            

            if (TimeStart > TimeEnd)
            {
                return "Time Start is bigger than Time End!";
            }

            if (!CheckTimeStart(TimeStart))
            {
                return "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
            }

            var appoinmentList = dentistSlot.Appointments.ToList();
            if (appoinmentList != null)
            {
                foreach (var ap in appoinmentList)
                {
                    TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                    TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                    if (IsOverlap(TimeStart.TimeOfDay, apStartTime, apEndTime))
                    {
                        return $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}";
                    }

                    if (IsOverlap(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                    {
                        return $"There is an appointment overlapping at " + $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay).ToString()}. Your appoinment needs 30'";
                    }
                }
            }
            appointment.ServiceId = serviceId;
            appointment.Status = "Done";
            appointment.DentistSlotId = dentistSlotId;
            appointment.TimeStart = TimeStart;
            appointment.TimeEnd = TimeEnd;

            appointmentRepo.UpdateAppointment(appointment);
            return "Success";
            }
            catch (Exception e)
            {
                return "Error while updating appointment!";
            }
            
        }

        public void PutAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "appointment cannot be null.");
            }

            try
            {
                var existing = appointmentRepo.GetAppointmentById(appointment.AppointmentId);
                if (existing == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Appointment with ID {appointment.AppointmentId} not found.");
                }

                appointmentRepo.UpdateAppointment(appointment);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating.", ex);
            }
        }

        public string DeleteAppointment(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return "Invalid appointment ID.";
            }

            try
            {
                var existing = appointmentRepo.GetAppointmentById(appointmentId);
                if (existing == null)
                {
                    return $"Appointment with ID {appointmentId} not found.";
                }
                appointmentRepo.DeleteAppointment(appointmentId);
                return "Success";
            }
            catch (Exception ex)
            {
                
                return "An error occurred while deleting the checkup schedule.";
            }
        }
        public string UpdateAppointments(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId,
            string status)
        {
            try
            {
                if (serviceId <= 0)
            {
                return "Service ID is empty!";
            }

            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
            if (sErvice == null)
            {
                return "This service is not existed!";
            }

            if (appointmentId <= 0)
            {
                return "Appointment ID is empty!";
            }
            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return "This appointment is not existed!";
            }

            if (dentistSlotId <= 0)
            {
                return "Dentist slot ID is empty!";
            }
            DentistSlot dentistSlot = dentistSlotService.GetDentistSlotById(dentistSlotId).Result;
            if (dentistSlot == null)
            {
                return "This dentist slot is not existed!";
            }

            if (TimeStart < dentistSlot.TimeStart || TimeEnd > dentistSlot.TimeEnd)
            {
                return "The time of this appointment is out of range for this dentist slot!";
            }

            var validStatuses = new[] { "Success", "Done", "Finished", "Processing", "Delete" };
            if (!validStatuses.Contains(status))
            {
                return "Status is not valid!";
            }

            if (TimeStart > TimeEnd)
            {
                return "Time Start is bigger than Time End!";
            }

            if (!CheckTimeStart(TimeStart))
            {
                return "Time Start must be in range [08:00-11:30] & [13:00-19:00]";
            }

            var appoinmentList = dentistSlot.Appointments.ToList();
            if (appoinmentList != null)
            {
                foreach (var ap in appoinmentList)
                {
                    if (!TimeStart.Equals(appointment.TimeStart))
                    {
                        TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                        TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                        if (IsOverlap(TimeStart.TimeOfDay, apStartTime, apEndTime))
                        {
                            return $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeEnd.TimeOfDay}";
                        }

                        if (IsOverlap(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                        {
                            return $"There is an appointment overlapping at " + $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay).ToString()}. Your appoinment needs 30'";
                        }
                    }
                }
            }
            appointment.ServiceId = serviceId;
            appointment.Status = status;
            appointment.DentistSlotId = dentistSlotId;
            appointment.TimeStart = TimeStart;
            appointment.TimeEnd = TimeEnd;

            appointmentRepo.UpdateAppointment(appointment);
            return "Success";
            }
            catch (Exception e)
            {
                return "Error while update appointment!";
            }
        }

        public List<string> GetAllStatusOfAppointment(int appointmentId)
        {
            List<string> statusList = new List<string> { "Success", "Done", "Finished", "Delete", "Processing"};
            if (appointmentId == 0)
            {
                return statusList;
            }
            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);

            var s = statusList.Where(s => s.Equals(appointment.Status)).FirstOrDefault();
            statusList.Remove(s);
            statusList.Insert(0, s);
            
            return statusList;
        }
    }
}
