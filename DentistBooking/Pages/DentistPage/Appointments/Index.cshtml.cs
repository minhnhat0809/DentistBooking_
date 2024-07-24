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
using X.PagedList;
using Microsoft.IdentityModel.Tokens;

namespace DentistBooking.Pages.DentistPage.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public IndexModel(IAppointmentService appointmentService, IUserService userService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
        }

        public IPagedList<AppointmentDto> Appointment { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                /*var role = HttpContext.Session.GetString("Role");
                if (role != "Dentist")
                {
                    return RedirectToPage("/Denied");
                }*/
                var role = HttpContext.Session.GetString("Role");
                string? email = HttpContext.Session.GetString("Email");
                if (!string.IsNullOrEmpty(email))
                {
                    var dentists = await _userService.GetAllDentists();
                    if (dentists != null)
                    {
                        var dentist = dentists.FirstOrDefault(x => x.Email == email);
                        if (dentist == null)
                        {
                            TempData["ErrorMessage"] = "No dentist found with the provided email.";
                            return RedirectToPage("/Error");
                        }
                        List<AppointmentDto> viewModels = await _appointmentService.GetAllAppointmentByDentistId(dentist.UserId);
                        Appointment = viewModels.ToPagedList(PageNumber, PageSize);
                        return Page();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No dentist found with the provided email.";
                        return RedirectToPage("/Error");
                    }
                }
                else return RedirectToPage("/Denied");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToPage("/Error");
            }
        }
    }
}
