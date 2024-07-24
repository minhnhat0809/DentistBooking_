using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDentistService
    {
        Task<List<ServiceDto>> GetAllServiceByDentist(int dentistId, int serviceId);

        Task<List<UserDto>> GetDentistsForAppointmentCustomer(int appointmentId);
    }
}
