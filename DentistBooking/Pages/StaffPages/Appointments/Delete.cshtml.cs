using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(IAppointmentService appointmentService, IHubContext<SignalRHub> hubContext)
        {
            _appointmentService = appointmentService;   
            _hubContext = hubContext;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

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
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id.Value);
            if (appointment != null)
            {
                Appointment = appointment;
            }
            _appointmentService.DeleteAppointment(id.Value);
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            return RedirectToPage("./Index");
        }
    }
}
