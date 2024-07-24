using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions
{
    public class CreateModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IPrescriptionService prescriptionService, IHubContext<SignalRHub> hubContext, IAppointmentService appointmentService)
        {
            _hubContext = hubContext;
            _prescriptionService = prescriptionService;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public PrescriptionDto Prescription { get; set; } = new PrescriptionDto();

        public async Task<IActionResult> OnGetAsync(int? appointmentId)
        {
            if (appointmentId == null)
            {
                return NotFound();
            }

            Prescription.AppointmentId = appointmentId.Value;
            ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId", Prescription.AppointmentId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId", Prescription.AppointmentId);
                return Page();
            }

            try
            {
                Prescription.Date = DateOnly.FromDateTime(DateTime.Now);
                Prescription.Status = true;
                Prescription.Total = 0;
                await _prescriptionService.CreatePrescription(Prescription);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptions");
                return RedirectToPage("/DentistPage/Appointments/Edit", new { id = Prescription.AppointmentId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId", Prescription.AppointmentId);
                return Page();
            }
        }
    }

}
