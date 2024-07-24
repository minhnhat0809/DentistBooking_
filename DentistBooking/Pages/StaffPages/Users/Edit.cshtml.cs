using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IClinicService _clinicService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IUserService userService, IClinicService clinicService, IHubContext<SignalRHub> hubContext)
        {
            _userService = userService;
            _clinicService = clinicService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public UserDto User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            ViewData["ClinicId"] = new SelectList(_clinicService.GetAllClinics().Result, "ClinicId", "ClinicName");
            ViewData["RoleId"] = new SelectList(_userService.GetAllRoles().Result, "RoleId", "RoleName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                ViewData["RoleId"] = new SelectList(_userService.GetAllRoles().Result, "RoleId", "RoleName");
                return Page();
            }

            try
            {
                await _userService.UpdateUser(User);
                await _hubContext.Clients.All.SendAsync("ReloadUsers");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(User.UserId))
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToPage("/Error");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the appointment: " + ex.Message);
                    ViewData["RoleId"] = new SelectList(_userService.GetAllRoles().Result, "RoleId", "RoleName");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                ViewData["RoleId"] = new SelectList(_userService.GetAllRoles().Result, "RoleId", "RoleName");
                return Page();
            }
        }

        private bool UserExists(int id)
        {
            return _userService.GetAllUsers().Result.Any(e => e.UserId == id);
        }
    }
}
