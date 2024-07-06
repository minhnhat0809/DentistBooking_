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

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class EditModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public EditModel(IClinicService clinicService)
        {
            _clinicService = clinicService; 
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
                _clinicService.UpdateClinic(Clinic);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(Clinic.ClinicId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ClinicExists(int id)
        {
            return _clinicService.GetAllClinics().Result.Any(e => e.ClinicId == id);
        }
    }
}
