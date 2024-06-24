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

        public AppointmentService(IAppointmentRepo appointmentRepo, IDentistSlotService dentistSlotService)
        {
            this.appointmentRepo = appointmentRepo;
            this.dentistSlotService = dentistSlotService;
        }

        public async Task<Dictionary<string, List<string>>> CreateAppointment(DateTime TimeStart, int dentistSlotId, int customerId)
        {
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            void AddError(string field, string message)
            {
                if (!errors.ContainsKey(field))
                {
                    errors[field] = new List<string>();
                }
                errors[field].Add(message);
            }

            if (dentistSlotId <= 0)
            {
                AddError("","Internal Error At Getting Dentist Slot!");
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
                    TimeSpan apEndTime = ap.Duration.ToTimeSpan();

                    if (IsOverlap(TimeStart.TimeOfDay, apStartTime, apEndTime))
                    {
                        AddError("TimeStart",
                            $"There is an appointment overlapping at {ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.Duration.ToTimeSpan()).ToString()}");
                        break; 
                    }

                    if (IsOverlap(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)), apStartTime, apEndTime))
                    {
                        AddError("TimeStart",
                            $"There is an appointment overlapping at " +
                            $"{ap.TimeStart.ToString()} - {ap.TimeStart.Add(ap.Duration.ToTimeSpan()).ToString()}. Your appoinment needs 30'");
                        break;
                    }
                }
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            Appointment appointment = new Appointment();
            appointment.TimeStart = TimeStart;
            appointment.Duration = TimeOnly.FromTimeSpan(TimeStart.TimeOfDay.Add(new TimeSpan(0, 30, 0)));
            appointment.DentistSlotId = dentistSlotId;
            appointment.CustomerId = customerId;
            appointment.Status = true;

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

        private bool IsOverlap(TimeSpan targetStart, TimeSpan apStart, TimeSpan apEnd)
        {
           
            return (targetStart >= apStart && targetStart < apEnd);
        }
    }
}
