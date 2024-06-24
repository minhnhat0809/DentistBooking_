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

namespace DentistBooking.Pages.Dentist_slots
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public EditModel(DataAccess.BookingDentistDbContext context)
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

            var dentistslot =  await _context.DentistSlots.FirstOrDefaultAsync(m => m.DentistSlotId == id);
            if (dentistslot == null)
            {
                return NotFound();
            }
            DentistSlot = dentistslot;
           ViewData["DentistId"] = new SelectList(_context.Users, "UserId", "Name");
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

            _context.Attach(DentistSlot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DentistSlotExists(DentistSlot.DentistSlotId))
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

        private bool DentistSlotExists(int id)
        {
            return _context.DentistSlots.Any(e => e.DentistSlotId == id);
        }
    }
}
