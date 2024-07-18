using System;
using System.Collections;
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
using BusinessObject.DTO;

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

        [BindProperty] public AppointmentDto Appointment { get; set; } = default!;

        public IList<string> Status { get; set; } = default!;

        public IList<MedicalRecordDto> MedicalRecords { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
            var dentistSlots = await _dentistSlotService.GetAllDentistSlots();
            var dentistSlotSelectList = dentistSlots.Select(slot => new
            {
                slot.DentistSlotId,
                DisplayText =
                    $"{slot.Dentist.Name} ({slot.TimeStart.ToString("HH:mm")} - {slot.TimeEnd.ToString("HH:mm")})"
            });
            ViewData["DentistSlotId"] = new SelectList(dentistSlotSelectList, "DentistSlotId", "DisplayText");
            Services = await _service.GetAllServicesForCustomer(appointment.ServiceId.Value);
            
            MedicalRecords = _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(appointment.CustomerId.Value).Result;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string result = await _appointmentService.UpdateAppointments(Appointment.ServiceId.Value,
                    Appointment.AppointmentId,
                    Appointment.TimeStart, Appointment.TimeEnd, Appointment.DentistSlotId.Value, Appointment.Status);
                if (!result.Equals("Success"))
                {
                    TempData["EditAppointment"] = result;
                }
                else
                {
                    TempData["EditAppointment"] = "Update successfully!";
                }

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

            return RedirectToPage(new { id = Appointment.AppointmentId });
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
        
    }
}
