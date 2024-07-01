using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;

namespace DentistBooking.Pages.Dentis.PrescriptionManagement
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public IndexModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        public IList<Prescription> Prescription { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Prescription = await _context.Prescriptions
                .Include(p => p.Appointment).ToListAsync();
        }
    }
}
