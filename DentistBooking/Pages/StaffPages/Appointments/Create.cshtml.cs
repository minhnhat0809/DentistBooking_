using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Service;
using Microsoft.AspNetCore.SignalR;

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
        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name");
            ViewData["DentistSlotId"] = new SelectList(_dentistSlotService.GetAllDentistSlots().Result, "DentistSlotId", "DentistSlotId");
            ViewData["ServiceId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            ViewData["MedicalRecordId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            return Page();
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public async Task<JsonResult> OnGetServicesByDentistSlotAsync(int dentistSlotId)
        {
            var services = await _service.GetServicesByDentistSlotAsync(dentistSlotId);
            var serviceList = services.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.ServiceName
            }).ToList();

            return new JsonResult(serviceList);
        }
        public async Task<JsonResult> OnGetMedicalRecordByCustomerIdAsync(int customerId)
        {
            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(customerId);
            var medicalRecordList = medicalRecords.Select(mr => new SelectListItem
            {
                Value = mr.MediaRecordId.ToString(),
                Text = mr.MediaRecordId.ToString()
            }).ToList();

            return new JsonResult(medicalRecordList);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name", Appointment.CustomerId);
                ViewData["DentistSlotId"] = new SelectList(_dentistSlotService.GetAllDentistSlots().Result, "DentistSlotId", "DentistSlotId", Appointment.DentistSlotId);
                ViewData["ServiceId"] = new SelectList(_service.GetAllServices().Result, "ServiceId", "ServiceName", Appointment.ServiceId);
                ViewData["MedicalRecordId"] = new SelectList(_medicalRecordService.GetAllMedicalRecords().Result, "MedicalRecordId", "MedicalRecordId", Appointment.MedicalRecordId);
                return Page();
            }

            _appointmentService.AddAppointment(Appointment);
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");

            return RedirectToPage("./Index");
        }
    }
}
