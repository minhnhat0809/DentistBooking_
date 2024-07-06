using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public CreateModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clinics.Add(Clinic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
