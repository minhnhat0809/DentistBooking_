using BusinessObject;
using BusinessObject.DTO;
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
        public IList<AppointmentDto> Appointments { get; set; } = default!;
        public async void OnGet()
        {
            Appointments =  appointmentService.GetAllProcessingAppointment().Result;
        }

        public async Task<IActionResult> OnPostDelete(int appointmentId)
        {

            string result = await appointmentService.DeleteAppointment(appointmentId);
            if (!result.Equals("Success"))
            {
                TempData["DeleteAppointment"] = result;
            }
            TempData["DeleteAppointment"] = "Delete successfully!";
            return RedirectToPage();
        }
    }
}
