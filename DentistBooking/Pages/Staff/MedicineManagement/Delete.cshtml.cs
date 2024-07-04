using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service;

namespace DentistBooking.Pages.Staff.MedicineManagement
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Medicine = _medicineService.GetById(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _medicineService.DeleteMedicine(id);

            return RedirectToPage("./Index");
        }
    }
}
