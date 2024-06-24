using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Repository;

namespace DentistBooking.Pages.Dentist_slots
{
    public class IndexModel : PageModel
    {
        private readonly IDentistSlotRepo dentistSlotService;

        public IndexModel(IDentistSlotRepo dentistSlotService)
        {
            this.dentistSlotService = dentistSlotService;
        }

        public IList<DentistSlot> DentistSlot { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DentistSlot = await dentistSlotService.GetAllDentistSlots();
        }
    }
}
