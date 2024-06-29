using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.CustomerPage
{
    public class AppointmentDetailModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;
        public AppointmentDetailModel(IAppointmentService appointmentService, IDentistService dentistService)
        {
            this.appointmentService = appointmentService;   
            this.dentistService = dentistService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public IList<BusinessObject.Service> Services { get; set; } = default!;
        public void OnGet(int id)
        {
            Appointment = appointmentService.GetAppointmentByID(id);
            Services = dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
        }

        public IActionResult OnPostUpdate()
        {

            Dictionary<string, string> result = appointmentService.UpdateAppointment((int)Appointment.ServiceId, 
                Appointment.AppointmentId, Appointment.TimeStart, (int)Appointment.CustomerId);
            if (!result.ContainsKey("Success"))
            {
                foreach(var item in result)
        {
                    TempData["AppointmentDetail"] = item.Value;
                }
                Services = dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
                return Page();
            }

            TempData["AppointmentDetail"] = "Appointment updated successfully!";
            Services = dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
            return Page();
        }
    }
}
