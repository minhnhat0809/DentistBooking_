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
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public DetailsModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

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
    }
}
