using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Result;

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

        Task<AppointmentResult> UpdateAppointmentForStaff(int serviceId, int appointmentId, DateTime TimeStart, DateTime TimeEnd, int dentistSlotId);
        Task<AppointmentResult> AddAppointment(AppointmentDto appointment, string email);
        Task PutAppointment(AppointmentDto appointment);   

        Task<string> DeleteAppointment(int appointmentId);
        
        Task<AppointmentResult> UpdateAppointments(AppointmentDto appointMent, string email);

        Task<List<string>> GetAllStatusOfAppointment(int appointmentId);

        AppointmentResult DeleteAppointmentForStaff(int appointmentId, string customerName, string reason);


        Task<List<AppointmentDto>> GetAllAppointmentByDentistId(int dentistId);
    }
}
