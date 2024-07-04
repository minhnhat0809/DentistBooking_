using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class CreateModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public CreateModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _medicineService.CreateMedicine(Medicine);

            return RedirectToPage("./Index");
        }
    }
}
