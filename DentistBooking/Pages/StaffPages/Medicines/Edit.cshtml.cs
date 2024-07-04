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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class EditModel : PageModel
    {
        private readonly IMedicineService _medicineService; 

        public EditModel(IMedicineService medicineService)
        {
            _medicineService = medicineService; 
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine =  _medicineService.GetById(id);
            if (medicine == null)
            {
                return NotFound();
            }
            Medicine = medicine;
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
                _medicineService.UpdateMedicine(Medicine);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(Medicine.MedicineId))
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

        private bool MedicineExists(int id)
        {
            return _medicineService.GetAllMedicines().Any(e => e.MedicineId == id);
        }
    }
}
