using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Runtime.CompilerServices;

namespace DentistBooking.Pages.CustomerPage
{
    public class BookModel : PageModel
    {
        private readonly IUserService userService;
        private readonly IDentistSlotService dentistSlotService;
        private readonly IAppointmentService appointmentService;

        public BookModel(IUserService userService, IDentistSlotService dentistSlotService, IAppointmentService appointmentService)
        {
            this.userService = userService;
            this.dentistSlotService = dentistSlotService;
            this.appointmentService = appointmentService;

        }
        [BindProperty]
        public DateOnly selectedDate {  get; set; }

        [BindProperty]
        public IList<User> selectedDentist { get; set; }

        [BindProperty(SupportsGet = true)]
        public int selectedDentistId { get; set; }

        public IList<DentistSlot> DentistSlots { get; set; }

        [BindProperty]
        public DateTime startTime { get; set; }

        [BindProperty]
        public int SlotId { get; set; }

        public async Task OnGetAsync(int id)
        {
            HttpContext.Session.SetString("ServiceId", id.ToString());
            selectedDentist = await userService.GetAllDentistsByService(id);
        }

        public async Task OnPostViewAsync()
        {
            selectedDentist = await userService.GetAllDentistsByService(Int32.Parse(HttpContext.Session.GetString("ServiceId")));
            DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentist(selectedDentistId);
            
        }

        public async Task<IActionResult> OnPostBookAsync()
        {
            if (!ModelState.IsValid)
            {
                selectedDentist = await userService.GetAllDentistsByService(Int32.Parse(HttpContext.Session.GetString("ServiceId")));
                DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentist(selectedDentistId);
                return Page();
            }
            Dictionary<string, List<string>> result = await appointmentService.CreateAppointment(startTime, SlotId , 2);
            selectedDentist = await userService.GetAllDentistsByService(Int32.Parse(HttpContext.Session.GetString("ServiceId")));
            DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentist(selectedDentistId);
            return Page();
        }
    }
}
