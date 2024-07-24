using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAppointmentRepo
    {
        Task<List<Appointment>> GetAllAppointments();

        Task CreateAppointment(Appointment appointment);

        Task<List<Appointment>> GetAllAppointmentsOfCustomer(int customerId);

        Task<Appointment> GetAppointmentById(int id);

        Task UpdateAppointment(Appointment appointment);
        Task DeleteAppointment(int appointmentId);

        Task<List<Appointment>> GetAllProcessingAppointment();

        Task<List<Appointment>> GetAllAppointmentsByDentist(int dentistId);
    }
}
