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

namespace DentistBooking.Pages.StaffPages.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public DetailsModel(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        public Clinic Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            
            var clinic = await _clinicService.GetById(1);
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
