
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;
using BusinessObject.Result;
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
       
        public IList<string> Status { get; set; } = default!;

        public IList<MedicalRecordDto> MedicalRecords { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeStart { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeEnd { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public DateOnly SelectedDate { get; set; } = default!;

        public IList<UserDto> Customers { get; set; } = default!;

        public IList<DentistSlot> DentistSlots { get; set; } = default!;
        
        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;
        
        public async Task<IActionResult> OnGet()
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
            List<UserDto> customers = (await _userService.GetAllActiveCustomers()).Users;
            Customers = customers;
            Status = await _appointmentService.GetAllStatusOfAppointment(0);
            Services = (await _service.GetAllActiveServices()).Services;
            DateTime now = DateTime.Now;

            DateTime noWw = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            DentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDateTime(Services.FirstOrDefault().ServiceId, noWw).DentistSlots;
            MedicalRecords = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(customers.FirstOrDefault().UserId);
            return Page();
        }
        public async Task<JsonResult> OnGetDentistSlotByServiceAsync(int serviceId, DateOnly selectedDate, TimeOnly timeStartt)
        {
            DateTime timeStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 
                timeStartt.Hour, timeStartt.Minute, timeStartt.Second);
            var dentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDateTime(serviceId, timeStart).DentistSlots;
            if (dentistSlots.IsNullOrEmpty())
            {
                return new JsonResult(new { success = false, message = "No dentist slots available for selected service and the selected time." });
            }
            var dentistSlotSelectList = dentistSlots.Select(slot => new SelectListItem
            {
                Value = slot.DentistSlotId.ToString(),
                Text =
                    $"{slot.Dentist.Name} ({slot.TimeStart.ToString("HH:mm")} - {slot.TimeEnd.ToString("HH:mm")})"
            }).ToList();

            return new JsonResult(new { success = true, dentistSlotSelectList });
        }
        public async Task<JsonResult> OnGetMedicalRecordByCustomerIdAsync(int customerId)
        {
            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(customerId);
            if (medicalRecords.IsNullOrEmpty())
            {
                return new JsonResult(new { success = false, message = "No medical records found" });
            }
            var medicalRecordList = medicalRecords.Select(mr => new SelectListItem
            {
                Value = mr.MediaRecordId.ToString(),
                Text = mr.Diagnosis
            }).ToList();

            return new JsonResult(new { success = true, medicalRecordList});
        }
        public async Task<IActionResult> OnPostAsync()
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
            
            AppointmentResult appointmentResult = await _appointmentService.AddAppointment(Appointment, email);
            if (!appointmentResult.Message.Equals("Success"))
            {
                TempData["ErrorCreateAppointment"] = appointmentResult.Message;
                List<UserDto> customers = (await _userService.GetAllActiveCustomers()).Users;
                Customers = customers;
                Status = await _appointmentService.GetAllStatusOfAppointment(0);
                Services = (await _service.GetAllActiveServices()).Services;
                DateTime now = DateTime.Now;

                DateTime noWw = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                DentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDateTime(Services.FirstOrDefault().ServiceId, noWw).DentistSlots;
                MedicalRecords = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(customers.FirstOrDefault().UserId);
                return Page();
            }
            else
            {
                TempData["SuccessCreateAppointment"] = "Create successfully!";
            }
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");

            return RedirectToPage("./Create");
        }
    }
}
