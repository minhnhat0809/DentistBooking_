using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;

namespace DentistBooking.Pages.Dentis
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public IndexModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        public IList<Appointment> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.DentistSlot)
                .Include(a => a.MedicalRecord)
                .Include(a => a.Service).ToListAsync();
        }
    }
}
