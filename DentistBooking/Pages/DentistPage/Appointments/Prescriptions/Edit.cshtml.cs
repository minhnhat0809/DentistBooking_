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
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions
{
    public class EditModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public EditModel(IPrescriptionService prescriptionService, IAppointmentService appointmentService,
            IHubContext<SignalRHub> hubContext)
        {
            _prescriptionService = prescriptionService;
            _appointmentService = appointmentService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public PrescriptionDto Prescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _prescriptionService.GetByIdWithMedicinesAsync(id.Value);
            prescription.PrescriptionMedicines = prescription.PrescriptionMedicines.Where(x=>x.Status == true).ToList();
            if (prescription == null)
            {
                return NotFound();
            }
            Prescription = prescription;
            ViewData["AppointmentId"] = new SelectList(_appointmentService.GetAllAppointments().Result, "AppointmentId", "AppointmentId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId");
                return Page();
            }

            try
            {
                await _prescriptionService.UpdatePrescription(Prescription);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptions");
                return RedirectToPage("/DentistPage/Appointments/Edit", new { id = Prescription.AppointmentId });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PrescriptionExists(Prescription.PrescriptionId))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message );
                    ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId");
                return Page();
            }
        }
        private bool PrescriptionExists(int id)
        {
            return _prescriptionService.GetPrescriptions().Result.Any(e => e.PrescriptionId == id);
        }

    }
}
