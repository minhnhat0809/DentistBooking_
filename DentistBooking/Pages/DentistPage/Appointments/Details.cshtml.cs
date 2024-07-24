using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Service;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPrescriptionService _prescriptionService;
        public DetailsModel(IAppointmentService appointmentService, IPrescriptionService prescriptionService)
        {
            _appointmentService = appointmentService;
            _prescriptionService = prescriptionService;
        }

        public AppointmentDto Appointment { get; set; } = default!;
        public PrescriptionDto Prescription { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // check null
            if (id != null)
            {
                try
                {
                    // check exist
                    var appointment = await _appointmentService.GetAppointmentByID(id.Value);
                    if (appointment == null)
                    {
                        TempData["ErrorMessage"] = "Appointment not found.";
                        return RedirectToPage("/Error");
                    }

                    Appointment = appointment;

                    // try get prescription
                    Prescription = await _prescriptionService.GetByAppointmentId(appointment.AppointmentId);

                    return Page();
                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                    return RedirectToPage("/Error");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Appointment ID is missing.";
                return RedirectToPage("/Error");
            }
        }
    }
}
