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

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public DetailsModel(IClinicService clinicService)
        {
            _clinicService = clinicService; 
        }

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

            var clinic = await _clinicService.GetById(id);
            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                Clinic = clinic;
            }
            return Page();
        }
    }
}
