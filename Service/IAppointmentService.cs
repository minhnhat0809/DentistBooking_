using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAppointments();

        Task<Dictionary<string, string>> CreateAppointment(DateTime TimeStart, int customerId, DateOnly selectedDate, int serviceId);

        Task<List<Appointment>> GetALlAppointmentsOfCustomer(int customerId);

        Appointment GetAppointmentByID(int id);

        Dictionary<string, string> UpdateAppointment(int serviceId, int appointmentId, DateTime TimeStart, int customerId);

        List<Appointment> GetAllProcessingAppointment();

        string UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId);
        string AddAppointment(Appointment appointment);
        void PutAppointment(Appointment appointment);   

        void DeleteAppointment(int appointmentId);
        
        string UpdateAppointments(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId, string status);

        List<string> GetAllStatusOfAppointment(int appointmentId);


    }
}
