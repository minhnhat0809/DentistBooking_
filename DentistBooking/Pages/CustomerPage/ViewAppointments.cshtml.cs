using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.CustomerPage
{
    public class ViewAppointmentsModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        
        public ViewAppointmentsModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public IList<AppointmentDto> Appointments { get; set; } = default!;


        public async Task OnGet()
        {
            var customerId = Int32.Parse(HttpContext.Session.GetString("ID"));
            Appointments = await appointmentService.GetALlAppointmentsOfCustomer(customerId);
        }

        public IActionResult OnPostDelete(int appointmentId)
        {
            var customerId = Int32.Parse(HttpContext.Session.GetString("ID"));
            Appointments = appointmentService.GetALlAppointmentsOfCustomer(customerId).Result;
            return Page();
        }
    }
}
