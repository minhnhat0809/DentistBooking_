using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class AppointmentRepo : IAppointmentRepo
    {
        public async Task CreateAppointment(Appointment appointment) => await AppointmentDAO.Instance.createAppointment(appointment);

        public async Task DeleteAppointment(int appointmentId)
        {
            var model = await AppointmentDAO.Instance.getAppointmnentByID(appointmentId);
            AppointmentDAO.Instance.deleteAppointment(model);
        }

        public async Task<List<Appointment>> GetAllAppointments() => await AppointmentDAO.Instance.getAllAppointments();

        public async Task<List<Appointment>> GetAllAppointmentsOfCustomer(int customerId) => await AppointmentDAO.Instance.getAllAppointmentsOfCustomer(customerId);

        public async Task<List<Appointment>> GetAllProcessingAppointment() => await AppointmentDAO.Instance.getAllProcessingAppointment();

        public async Task<Appointment> GetAppointmentById(int id) => await AppointmentDAO.Instance.getAppointmnentByID(id);

        public async Task UpdateAppointment(Appointment appointment) => AppointmentDAO.Instance.updateAppointment(appointment);
    }
}
