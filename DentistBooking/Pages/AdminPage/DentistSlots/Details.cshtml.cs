using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using BusinessObject.DTO;

namespace DentistBooking.Pages.AdminPage.DentistSlots
{
    public class DetailsModel : PageModel
    {
        private readonly IDentistSlotService _dentistSlotService;

        public DetailsModel(IDentistSlotService dentistSlotService)
        {
            _dentistSlotService = dentistSlotService;
        }

        public DentistSlotDto DentistSlot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dentistslot = await _dentistSlotService.GetDentistSlotById(id.Value);
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
