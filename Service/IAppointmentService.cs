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

        Task<Dictionary<string, List<string>>> CreateAppointment(DateTime TimeStart, int dentistSlotId, int customerId);

        Task<List<Appointment>> GetALlAppointmentsOfCustomer(int customerId);
    }
}
