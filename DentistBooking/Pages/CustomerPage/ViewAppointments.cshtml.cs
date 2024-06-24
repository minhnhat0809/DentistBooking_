using BusinessObject;
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

        public IList<Appointment> Appointments { get; set; } = default!;


        public async Task OnGet()
        {
            Appointments = await appointmentService.GetALlAppointmentsOfCustomer(2);
        }
    }
}
