using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.Staff.MedicineManagement
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

          
            Medicine = _medicineService.GetById(id);
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

            _medicineService.UpdateMedicine(Medicine);

            return RedirectToPage("./Index");
        }

    }
}
