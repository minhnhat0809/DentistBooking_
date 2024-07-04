using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;

namespace DentistBooking.Pages.StaffPages.Appointments
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
        ViewData["CustomerId"] = new SelectList(_context.Users, "UserId", "Name");
        ViewData["DentistSlotId"] = new SelectList(_context.DentistSlots, "DentistSlotId", "DentistSlotId");
        ViewData["MedicalRecordId"] = new SelectList(_context.MedicalRecords, "MediaRecordId", "MediaRecordId");
        ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return Page();
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
