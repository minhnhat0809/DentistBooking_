using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
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
        
        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }
        public async void OnGet()
        {
            Appointments =  appointmentService.GetAllProcessingAppointment().Result;
        }

        public async Task<IActionResult> OnPostDelete(int appointmentId)
        {
            if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Reason and Customer Name are required.";
                return RedirectToPage("/StaffPages/ProcessingAppointmentList");
            }

            AppointmentResult result = appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, Reason);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteAppointment"] = result.Message;
                Appointments =  appointmentService.GetAllProcessingAppointment().Result;
                return Page();
            }
            TempData["SuccessDeleteAppointment"] = "Delete successfully!";
            return RedirectToPage();
        }
    }
}
