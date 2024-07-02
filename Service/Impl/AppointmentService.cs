using BusinessObject;
using Repository;
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

            /*if (dentistSlotId <= 0)
            {
                AddError("","Internal Error At Getting Dentist Slot!");
                return errors;
            }

            DentistSlot dentistSlot = await dentistSlotService.GetDentistSlot(dentistSlotId);
            if (dentistSlot == null)
            {
                AddError("", "Internal Error At Finding Dentist Slot!");
                return errors;
            }

            List<Appointment> appointments = dentistSlot.Appointments.ToList();

            if (appointments != null)
            {
                foreach (var ap in appointments)
                {
                    TimeSpan apStartTime = ap.TimeStart.TimeOfDay;
                    TimeSpan apEndTime = ap.TimeEnd.TimeOfDay;

                    if (IsOverlap(TimeStart.TimeOfDay, apStartTime, apEndTime))
                    {
                        AddError("TimeStart",
                            $"There is an appointment overlapping at {ap.TimeStart} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay)}");
                        break; 
                    }

                    if (IsOverlap(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                    {
                        AddError("TimeStart",
                            $"There is an appointment overlapping at " +
                            $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.TimeEnd.TimeOfDay).ToString()}. Your appoinment needs 30'");
                        break;
                    }
                }
            }*/

            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
            if (sErvice == null)
            {
                AddError("Service","Service is not existed!");
                return errors;
            }

            if (!CheckTimeStart(TimeStart))
            {
                AddError("TimeStart","Time must be in range [8:00-11:30] & [13:00-19:00]");
                return errors;
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

            if (serviceId <= 0)
            {
                AddError("Service", "Service Id is empty!");
                return errors;
            }

            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);
            if (sErvice == null)
            {
                AddError("Service","Service is not existed!");
                return errors;
            }

            if (customerId <= 0)
            {
                AddError("Customer", "Cusotmer Id is empty!");
                return errors;
            }

            User cusomter = userRepo.GetById(customerId);
            if (cusomter == null)
            {
                AddError("Customer","Customer is not existed!");
                return errors;
            }

            if (!CheckTimeStart(TimeStart))
            {
                AddError("TimeStart","Time must be in range [8:00-11:30] & [13:00-19:00]");
                return errors;
            }

            Appointment appointment = appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                AddError("Appointment","Appointment is not existed!");
                return errors;
            }

            switch (appointment.Status)
            {
                case "Deleted":
                    AddError("Status","Cannot update! This appointment is deleted!");
                    return errors;
                case "Happening":
                    AddError("Status", "Cannot update! This appointment is happening!");
                    return errors;
                case "Expired":
                    AddError("Status", "Cannot update! This appointment is expired!");
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

        public Dictionary<string, string> UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId)
        {
            throw new NotImplementedException();
        }
    }
}
