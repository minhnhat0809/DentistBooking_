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
using BusinessObject.DTO;

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
        public async Task<IActionResult> OnGet()
        {
            ViewData["CustomerId"] = new SelectList( await _userService.GetAllUsers(), "UserId", "Name");
            Status = await _appointmentService.GetAllStatusOfAppointment(0);
            var dentistSlots = await  _dentistSlotService.GetAllDentistSlots();
            var dentistSlotSelectList = dentistSlots.Select(slot => new
            {
                slot.DentistSlotId,
                DisplayText =
                    $"{slot.Dentist.Name} ({slot.TimeStart.ToString("HH:mm")} - {slot.TimeEnd.ToString("HH:mm")})"
            });
            ViewData["DentistSlotId"] = new SelectList(dentistSlotSelectList, "DentistSlotId", "DisplayText");

            ViewData["ServiceId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");

            return Page();
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

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
                Text = mr.Customer.Name +"-"+ mr.Diagnosis
            }).ToList();

            return new JsonResult(medicalRecordList);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            string result = await _appointmentService.AddAppointment(Appointment);
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
