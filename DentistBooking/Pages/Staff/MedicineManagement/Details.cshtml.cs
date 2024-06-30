using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public DetailsModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        public Medicine Medicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.MedicineId == id);
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
    }
}
