using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Runtime.CompilerServices;

namespace DentistBooking.Pages.CustomerPage
{
    public class BookModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public BookModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;

        }
        [BindProperty]
        public DateOnly selectedDate {  get; set; }


        [BindProperty]
        public DateTime startTime { get; set; }

        [BindProperty]
        public int serviceId { get; set; }


        public void OnGet(int id)
        {
            serviceId = id;
        }

        public async Task<IActionResult> OnPostBookAsync()
        {
            var customerId = Int32.Parse(HttpContext.Session.GetString("ID"));

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Dictionary<string, string> result = await appointmentService.CreateAppointment(startTime, customerId, selectedDate, serviceId);

            if (!result.ContainsKey("Success"))
            {
                foreach (var item in result)
                {
                    TempData["Book"] = item.Value;
                }
                return Page();
            }
            TempData["Book"] = "Appointment created successfully!";
            return Page();
        }
    }
}
