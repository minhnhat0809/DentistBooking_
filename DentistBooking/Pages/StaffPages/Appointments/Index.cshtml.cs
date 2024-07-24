using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using X.PagedList;
using BusinessObject.DTO;
using Microsoft.IdentityModel.Tokens;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IPagedList<AppointmentDto> Appointment { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            string role = HttpContext.Session.GetString("Role");
            if (!role.IsNullOrEmpty())
            {
                if (!role.Equals("Staff"))
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/Index");
            }
            var appointments = await _appointmentService.GetAllAppointments();
            appointments = appointments.Where(ap => !ap.Status.Equals("Processing")).ToList();
            Appointment = appointments.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
