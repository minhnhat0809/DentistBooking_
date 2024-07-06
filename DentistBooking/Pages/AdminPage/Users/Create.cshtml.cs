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

namespace DentistBooking.Pages.AdminPage.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IClinicService _clinicService;
        public CreateModel(IUserService userService, IClinicService clinicService)
        {
            _userService = userService;
            _clinicService = clinicService;
        }

        public IActionResult OnGet()
        {
            ViewData["ClinicId"] = new SelectList( _clinicService.GetAllClinics().Result, "ClinicId", "ClinicName");
            ViewData["RoleId"] = new SelectList( _userService.GetAllRoles().Result, "RoleId", "RoleName");
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
            User.CreatedDate = DateTime.Now;
            User.Status = true;
            _userService.CreateUser(User);

            return RedirectToPage("./Index");
        }
    }
}
