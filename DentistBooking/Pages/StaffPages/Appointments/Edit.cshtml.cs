using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IDentistSlotService _dentistSlotService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IService _service;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IAppointmentService appointmentService,
            IUserService userService, IDentistSlotService dentistSlotService,
            IMedicalRecordService medicalRecordService, IService service,
            IHubContext<SignalRHub> hubContext)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _medicalRecordService = medicalRecordService;
            _service = service;
            _hubContext = hubContext;   
        }

        [BindProperty] 
        public AppointmentDto Appointment { get; set; } = default!;

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
        
        public async Task<IActionResult> OnGetAsync(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            Appointment = appointment;
            
            Status = await _appointmentService.GetAllStatusOfAppointment(appointment.AppointmentId);

            var dentistSlots = _dentistSlotService.GetDentistSlotByServiceAndDateTime(appointment.ServiceId.Value, appointment.TimeStart).DentistSlots;

            DentistSlots = _dentistSlotService.GetDentistSlotForAppointment(dentistSlots, appointment.AppointmentId).DentistSlots;
            
            Services = await _service.GetAllServicesForCustomer(appointment.ServiceId.Value);
            
            MedicalRecords = _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(appointment.CustomerId.Value).Result;
            
            return Page();
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
                return RedirectToPage(new {id = Appointment.AppointmentId});
            }
            DateTime timeStart = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
            DateTime timeEnd = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);

            Appointment.TimeStart = timeStart;
            Appointment.TimeEnd = timeEnd;
            
            string email = HttpContext.Session.GetString("Email");

            AppointmentResult result = await _appointmentService.UpdateAppointments(Appointment, email);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorEditAppointment"] = result.Message;
            }
            else
            {
                TempData["SuccessEditAppointment"] = "Update successfully!";
                await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            }

            return RedirectToPage(new { id = Appointment.AppointmentId });
        }

        public JsonResult OnGetDentistSlotByServiceAsync(int serviceId, DateOnly selectedDate, TimeOnly timeStart)
        {
            DateTime timeStartt = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 
                timeStart.Hour, timeStart.Minute, timeStart.Second);
            var dentistSlots =  _dentistSlotService.GetDentistSlotByServiceAndDateTime(serviceId, timeStartt).DentistSlots;
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
        
    }
}
