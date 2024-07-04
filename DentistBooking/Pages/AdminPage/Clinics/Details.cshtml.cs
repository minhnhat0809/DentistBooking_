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

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public DetailsModel(IClinicService clinicService)
        {
            _clinicService = clinicService; 
        }

        public Clinic Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
