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

        public async Task<List<Appointment>> GetAllAppointments() => await AppointmentDAO.Instance.getAllAppointments();

        public async Task<List<Appointment>> GetAllAppointmentsOfCustomer(int customerId) => await AppointmentDAO.Instance.getAllAppointmentsOfCustomer(customerId);

        public List<Appointment> GetAllProcessingAppointment() => AppointmentDAO.Instance.getAllProcessingAppointment();

        public Appointment GetAppointmentById(int id) => AppointmentDAO.Instance.getAppointmnentByID(id);

        public void UpdateAppointment(Appointment appointment) => AppointmentDAO.Instance.updateAppointment(appointment);
    }
}
