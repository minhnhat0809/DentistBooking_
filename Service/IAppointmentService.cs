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

        Task<Dictionary<string, List<string>>> CreateAppointment(DateTime TimeStart, int dentistSlotId, int customerId, int serviceId);

        Task<List<Appointment>> GetALlAppointmentsOfCustomer(int customerId);

        Dictionary<string, string> DeleteAppointment(int appointmentId);

        Appointment GetAppointmentByID(int appointmentId);

        List<BusinessObject.Service> GetServiceOfDentistByDentistSlotID(int dentistSlotId, int serviceId);

        Dictionary<string, string> UpdateAppointment(int appointmentId, DateTime TimeStart, int serviceId);
    }
}
