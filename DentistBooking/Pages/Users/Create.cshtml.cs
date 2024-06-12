using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service;

namespace DentistBooking.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService userService;

        public CreateModel(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult OnGet()
        {
            //ViewData["ClinicId"] = new SelectList(clinicService., "ClinicId", "Address");
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            userService.CreateUser(User);

            return RedirectToPage("./Index");
        }
    }
}
