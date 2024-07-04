using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IDentistSlotService _dentistSlotService;
        private readonly IService _service;
        private readonly IMedicalRecordService _medicalRecordService;

        public EditModel(IAppointmentService appointmentService, IUserService userService, IDentistSlotService dentistSlotService, IService service, IMedicalRecordService medicalRecordService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _service = service;
            _medicalRecordService = medicalRecordService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Appointment = _appointmentService.GetAppointmentByID(id.Value);

            if (Appointment == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name", Appointment.CustomerId);
            ViewData["DentistSlotId"] = new SelectList(_dentistSlotService.GetAllDentistSlots().Result, "DentistSlotId", "DentistSlotId", Appointment.DentistSlotId);
            ViewData["ServiceId"] = new SelectList(await _service.GetAllServices(), "ServiceId", "ServiceName", Appointment.ServiceId);
            ViewData["MedicalRecordId"] = new SelectList(await _medicalRecordService.GetAllMedicalRecords(), "MedicalRecordId", "MedicalRecordId", Appointment.MedicalRecordId);

            return Page();
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

            try
            {
                _appointmentService.PutAppointment(Appointment);
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                throw;
            }

            return RedirectToPage("./Index");
        }

        
    }
}
