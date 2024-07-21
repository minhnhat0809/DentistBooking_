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
using BusinessObject.Result;
using Microsoft.IdentityModel.Tokens;

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
        
        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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

        public async Task<IActionResult> OnPostAsync(int appointmentId)
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

            if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Reason and Customer Name are required.";
                Appointment = await _appointmentService.GetAppointmentByID(appointmentId);
                return Page();
            }

            AppointmentResult result = _appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, Reason);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteAppointment"] = result.Message;
                Appointment =  await _appointmentService.GetAppointmentByID(appointmentId);
                return Page();
            }
            TempData["SuccessDeleteAppointment"] = "Delete successfully!";
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            return RedirectToPage("./Index");
        }
    }
}
