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
    public class indexModel : PageModel
    {
        private readonly DataAccess.BookingDentistDbContext _context;

        public indexModel(DataAccess.BookingDentistDbContext context)
        {
            _context = context;
        }

        public IList<PrescriptionMedicine> PrescriptionMedicine { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PrescriptionMedicine = await _context.PrescriptionMedicines
                .Include(p => p.Medicine)
                .Include(p => p.Prescription).ToListAsync();
        }
    }
}
