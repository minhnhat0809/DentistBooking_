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
using BusinessObject.DTO;
using Microsoft.IdentityModel.Tokens;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public DetailsModel(IAppointmentService appointmentService)
        { 
            _appointmentService = appointmentService;   
        }

        public AppointmentDto Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (!role.IsNullOrEmpty())
            {
                if (!role.Equals("Staff"))
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/Index");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id);
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
    }
}
