using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public DeleteModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                Medicine = medicine;
                _context.Medicines.Remove(Medicine);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
