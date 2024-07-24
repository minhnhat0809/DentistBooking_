using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.CustomerPage
{
    public class AppointmentDetailModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;
        private readonly IService service;
        public AppointmentDetailModel(IAppointmentService appointmentService, IDentistService dentistService, IService service)
        {
            this.appointmentService = appointmentService;   
            this.dentistService = dentistService;
            this.service = service;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;
        public async Task<IActionResult>  OnGet(int id)
        {
            Appointment = await appointmentService.GetAppointmentByID(id);
            if (Appointment.DentistSlot != null)
            {
                Services = await dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
                return Page();
            }
            else
            {
                Services = await service.GetAllServicesForCustomer((int)Appointment.ServiceId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {

            Dictionary<string, string> result = await appointmentService.UpdateAppointment((int)Appointment.ServiceId, 
                Appointment.AppointmentId, Appointment.TimeStart, (int)Appointment.CustomerId);
            if (!result.ContainsKey("Success"))
            {
                foreach(var item in result)
        {
                    TempData["AppointmentDetail"] = item.Value;
                }
                Appointment = await appointmentService.GetAppointmentByID(Appointment.AppointmentId);
                if (Appointment.DentistSlot != null)
                {
                    Services = await dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
                }
                else
                {
                    Services = await service.GetAllServicesForCustomer((int)Appointment.ServiceId);
                }
                return Page();
            }

            TempData["AppointmentDetail"] = "Appointment updated successfully!";
            Appointment = await appointmentService.GetAppointmentByID(Appointment.AppointmentId);
            if (Appointment.DentistSlot != null)
            {
                Services = await dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
            }
            else
            {
                Services = await service.GetAllServicesForCustomer((int)Appointment.ServiceId);
            }
            return Page();
        }
    }
}
