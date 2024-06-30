using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public EditModel(DataAccess.BookingDentistDbContext context)
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

            var medicine =  await _context.Medicines.FirstOrDefaultAsync(m => m.MedicineId == id);
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

            _context.Attach(Medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }
}
