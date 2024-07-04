using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public DetailsModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

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
    }
}
