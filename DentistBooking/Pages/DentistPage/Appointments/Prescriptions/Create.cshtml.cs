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

        public IActionResult OnGet()
        {
        ViewData["AppointmentId"] = new SelectList(_appointmentService.GetAllAppointments().Result, "AppointmentId", "AppointmentId");
            return Page();
        }

        [BindProperty]
        public PrescriptionDto Prescription { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["AppointmentId"] = new SelectList(_appointmentService.GetAllAppointments().Result, "AppointmentId", "AppointmentId");
                return Page();
            }
            Prescription.Date = DateOnly.FromDateTime(DateTime.Now);
            Prescription.Status = true;
            await _prescriptionService.CreatePrescription(Prescription);
            await _hubContext.Clients.All.SendAsync("ReloadPrescriptions");

            return RedirectToPage("./Index");
        }
    }
}
