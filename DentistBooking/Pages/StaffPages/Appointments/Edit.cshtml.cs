using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService; 
        private readonly IDentistSlotService _dentistSlotService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IService _service;

        public EditModel(IAppointmentService appointmentService, 
            IUserService userService, IDentistSlotService dentistSlotService, 
            IMedicalRecordService medicalRecordService, IService service)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _medicalRecordService = medicalRecordService;
            _service = service;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = _appointmentService.GetAppointmentByID(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            Appointment = appointment;
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name");
            ViewData["DentistSlotId"] = new SelectList(_dentistSlotService.GetAllDentistSlots().Result, "DentistSlotId", "DentistSlotId");
            ViewData["ServiceId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            ViewData["MedicalRecordId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
             
            try
            {
                _appointmentService.PutAppointment(Appointment);    
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(Appointment.AppointmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AppointmentExists(int id)
        {
            return _appointmentService.GetAllAppointments().Result.Any(e => e.AppointmentId == id);
        }
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
    }
}
