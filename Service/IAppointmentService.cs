using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAppointments();

        Task<Dictionary<string, string>> CreateAppointment(DateTime TimeStart, int customerId, DateOnly selectedDate, int serviceId);

        Task<List<AppointmentDto>> GetALlAppointmentsOfCustomer(int customerId);

        Task<AppointmentDto> GetAppointmentByID(int id);

        Task<Dictionary<string, string>> UpdateAppointment(int serviceId, int appointmentId, DateTime TimeStart, int customerId);

        Task<List<AppointmentDto>> GetAllProcessingAppointment();

        Task<string> UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId);
        Task<string> AddAppointment(AppointmentDto appointment, string email);
        Task PutAppointment(AppointmentDto appointment);   

        Task<string> DeleteAppointment(int appointmentId);
        
        Task<string> UpdateAppointments(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId, string status);

        Task<List<string>> GetAllStatusOfAppointment(int appointmentId);

        Task<List<AppointmentDto>> GetAllAppointmentByDentistId(int dentistId);
    }
}
