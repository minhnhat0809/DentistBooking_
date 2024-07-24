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
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;
using Service.Impl;

namespace DentistBooking.Pages.StaffPages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IClinicService _clinicService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public CreateModel(IUserService userService, IClinicService clinicService, IHubContext<SignalRHub> hubContext)
        {
            _userService = userService;
            _clinicService = clinicService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Staff")
            {
                return RedirectToPage("/Denied");
            }
            PopulateSelectLists();
            return Page();
        }

        [BindProperty]
        public UserDto User { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Populate dropdowns or other select lists
                ViewData["RoleId"] = new SelectList(await _userService.GetAllRoles(), "RoleId", "RoleName");
                return Page();
            }

            try
            {
                User.CreatedDate = DateTime.Now;
                User.Status = true;

                await _userService.CreateUser(User);

                await _hubContext.Clients.All.SendAsync("ReloadUsers");

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Handle exceptions and provide feedback
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                ViewData["RoleId"] = new SelectList(await _userService.GetAllRoles(), "RoleId", "RoleName");
                return Page();
            }
        }

        private void PopulateSelectLists()
        {
            ViewData["ClinicId"] = new SelectList(_clinicService.GetAllClinics().Result, "ClinicId", "ClinicName");
            ViewData["RoleId"] = new SelectList(_userService.GetAllRoles().Result, "RoleId", "RoleName");
        }
    }
}
