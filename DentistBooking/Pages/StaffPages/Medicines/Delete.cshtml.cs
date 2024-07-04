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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public DeleteModel(IMedicineService medicineService)
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
            else
            {
                Medicine = medicine;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = _medicineService.GetById(id);
            if (medicine != null)
            {
                Medicine = medicine;
                _medicineService.DeleteMedicine(Medicine.MedicineId);
                // signalR real-time
            }

            return RedirectToPage("./Index");
        }
    }
}
