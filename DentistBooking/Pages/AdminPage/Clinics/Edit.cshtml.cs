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
using BusinessObject.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Drawing.Printing;
using X.PagedList;

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class EditModel : PageModel
    {
        private readonly IClinicService _clinicService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IClinicService clinicService, IHubContext<SignalRHub> hubContext)
        {
            _clinicService = clinicService; 
            _hubContext = hubContext;
        }

        [BindProperty]
        public ClinicDto Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
            if (id == null)
            {
                return NotFound();
            }

            var clinic =  await _clinicService.GetById(id);
            if (clinic == null)
            {
                return NotFound();
            }
            Clinic = clinic;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _clinicService.UpdateClinic(Clinic);
                await _hubContext.Clients.All.SendAsync("ReloadClinics");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(Clinic.ClinicId))
                {
                    // If the clinic no longer exists, return NotFound
                    return NotFound();
                }
                else
                {
                    // Otherwise, rethrow the exception
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private bool ClinicExists(int id)
        {
            return _clinicService.GetAllClinics().Result.Any(e => e.ClinicId == id);
        }
    }
}
