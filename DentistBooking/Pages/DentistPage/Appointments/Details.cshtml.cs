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
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
                Prescription = await _prescriptionService.GetByAppointmentId(appointment.AppointmentId);
            }
            return Page();
        }
    }
}
