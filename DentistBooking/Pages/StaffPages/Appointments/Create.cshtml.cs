
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;
using Microsoft.IdentityModel.Tokens;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IDentistSlotService _dentistSlotService;
        private readonly IService _service;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IAppointmentService appointmentService, 
            IUserService userService, IDentistSlotService dentistSlotService,
            IService service, IMedicalRecordService medicalRecordService, 
            IHubContext<SignalRHub> hubContext)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _service = service;
            _medicalRecordService = medicalRecordService;
            _hubContext = hubContext;
        }
        /*
         flow: pick dentist slot (GetAllDentistSlot) 
	        -> Pick Service (GetAllServiceByDentist)
	        -> choose Customer (GetAllCustomer) 
	        -> medical record auto pick by customerID (GetMedicalRecordByCustomerId)
        */
        public IList<string> Status { get; set; } = default!;

        public IList<MedicalRecordDto> MedicalRecords { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeStart { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeEnd { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public DateOnly SelectedDate { get; set; } = default!;

        public IList<DentistSlot> DentistSlots { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            ViewData["CustomerId"] = new SelectList( await _userService.GetAllCustomers(), "UserId", "Name");
            Status = await _appointmentService.GetAllStatusOfAppointment(0);
            Services = await _service.GetAllServices();
            DateTime now = DateTime.Now;
            DentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDate(Services.FirstOrDefault().ServiceId, now).DentistSlots;
            return Page();
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public async Task<JsonResult> OnGetDentistSlotByServiceAsync(int serviceId, DateOnly selectedDate, TimeOnly timeStartt)
        {
            DateTime timeStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 
                timeStartt.Hour, timeStartt.Minute, timeStartt.Second);
            var dentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDateTime(serviceId, timeStart).DentistSlots;
            if (dentistSlots.IsNullOrEmpty())
            {
                return new JsonResult("");
            }
            var dentistSlotSelectList = dentistSlots.Select(slot => new SelectListItem
            {
                Value = slot.DentistSlotId.ToString(),
                Text =
                    $"{slot.Dentist.Name} ({slot.TimeStart.ToString("HH:mm")} - {slot.TimeEnd.ToString("HH:mm")})"
            }).ToList();

            return new JsonResult(dentistSlotSelectList);
        }
        public async Task<JsonResult> OnGetMedicalRecordByCustomerIdAsync(int customerId)
        {
            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(customerId);
            if (medicalRecords.IsNullOrEmpty())
            {
                return new JsonResult("");
            }
            var medicalRecordList = medicalRecords.Select(mr => new SelectListItem
            {
                Value = mr.MediaRecordId.ToString(),
                Text = mr.Diagnosis
            }).ToList();

            return new JsonResult(medicalRecordList);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            DateTime timeStart = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
            DateTime timeEnd = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);

            Appointment.TimeStart = timeStart;
            Appointment.TimeEnd = timeEnd;
            string email = HttpContext.Session.GetString("Email");
            string result = await _appointmentService.AddAppointment(Appointment, email);
            if (result.Equals("Success"))
            {
                TempData["CreateAppointment"] = result;
            }

            TempData["CreateAppointment"] = "Create successfully!";
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");

            return RedirectToPage("./Create");
        }
    }
}
