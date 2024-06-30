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

        Appointment GetAppointmentById(int id);

        void UpdateAppointment(Appointment appointment);   
    }
}
