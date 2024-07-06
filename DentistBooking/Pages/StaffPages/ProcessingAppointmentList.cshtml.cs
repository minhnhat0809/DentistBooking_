using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.StaffPages
{
    public class ProcessingAppointmentListModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public ProcessingAppointmentListModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [BindProperty]
        public IList<Appointment> Appointments { get; set; } = default!;
        public void OnGet()
        {
            Appointments = appointmentService.GetAllProcessingAppointment();
        }

        public IActionResult OnPostDelete(int appointmentId)
        {

            string result = appointmentService.DeleteAppointment(appointmentId);
            if (!result.Equals("Success"))
            {
                TempData["DeleteAppointment"] = result;
            }
            TempData["DeleteAppointment"] = "Delete successfully!";
            return RedirectToPage();
        }
    }
}
