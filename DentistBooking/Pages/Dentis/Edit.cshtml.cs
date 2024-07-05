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

namespace DentistBooking.Pages.Dentis
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public EditModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PrescriptionMedicine PrescriptionMedicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionmedicine =  await _context.PrescriptionMedicines.FirstOrDefaultAsync(m => m.PrescriptionMedicineId == id);
            if (prescriptionmedicine == null)
            {
                return NotFound();
            }
            PrescriptionMedicine = prescriptionmedicine;
           ViewData["MedicineId"] = new SelectList(_context.Medicines, "MedicineId", "MedicineName");
           ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "PrescriptionId");
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

            _context.Attach(PrescriptionMedicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionMedicineExists(PrescriptionMedicine.PrescriptionMedicineId))
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

        private bool PrescriptionMedicineExists(int id)
        {
            return _context.PrescriptionMedicines.Any(e => e.PrescriptionMedicineId == id);
        }
    }
}
