using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;

namespace DentistBooking.Pages.Dentist_slots
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public DeleteModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DentistSlot DentistSlot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dentistslot = await _context.DentistSlots.FirstOrDefaultAsync(m => m.DentistSlotId == id);

            if (dentistslot == null)
            {
                return NotFound();
            }
            else
            {
                DentistSlot = dentistslot;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dentistslot = await _context.DentistSlots.FindAsync(id);
            if (dentistslot != null)
            {
                DentistSlot = dentistslot;
                _context.DentistSlots.Remove(DentistSlot);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
