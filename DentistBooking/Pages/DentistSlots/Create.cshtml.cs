using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Service;

namespace DentistBooking.Pages.Dentist_slots
{
    public class CreateModel : PageModel
    {
        private readonly IDentistSlotService dentistSlotService;
        private readonly IUserService userService;

        public CreateModel(IDentistSlotService dentistSlotService, IUserService userService)
        {
            this.dentistSlotService = dentistSlotService;
            this.userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            List<User>? dentistList = await userService.GetAllDentists();

        ViewData["DentistId"] = new SelectList(dentistList, "UserId", "Name");
            return Page();
        }

        [BindProperty]
        public DentistSlot DentistSlot { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            return RedirectToPage("./Index");
        }
    }
}
