using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Pages.DentistPage.Prescriptions
{
    public class DetailsModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;

        public DetailsModel(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public Prescription Prescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _prescriptionService.GetByIdWithMedicinesAsync(id.Value);
            if (prescription == null)
            {
                return NotFound();
            }
            else
            {
                Prescription = prescription;
            }
            return Page();
        }
    }
}
