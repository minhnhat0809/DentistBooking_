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
        private readonly IService service;

        public BookModel(IAppointmentService appointmentService, IService service)
        {
            this.appointmentService = appointmentService;
            this.service = service;

        }
        [BindProperty]
        public DateOnly selectedDate {  get; set; }


        [BindProperty]
        public DateTime startTime { get; set; }

        [BindProperty]
        public int serviceId { get; set; }

        public IList<BusinessObject.Service> Services { get; set; } = default!;


        public void OnGet(int id)
        {
            serviceId = id;
            Services = service.GetAllServicesForCustomer(serviceId);
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
            return RedirectToPage(new {id = serviceId});
        }
    }
}
