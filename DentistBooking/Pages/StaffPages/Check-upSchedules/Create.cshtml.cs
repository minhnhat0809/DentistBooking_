using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Repository;
using Service;

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class CreateModel : PageModel
    {
        private readonly ICheckupScheduleService _checkupScheduleService; 
        private readonly IUserService _userService;
        private readonly IDentistService _dentistService;

        public CreateModel(ICheckupScheduleService checkupScheduleService, IUserService userService, IDentistService dentistService)
        {
            _checkupScheduleService = checkupScheduleService;
            _userService = userService;
            _dentistService = dentistService;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name");
        ViewData["DentistId"] = new SelectList(_userService.GetAllDentists().Result, "UserId", "Name");
            return Page();
        }

        [BindProperty]
        public CheckupSchedule CheckupSchedule { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _checkupScheduleService.CreateCheckupSchedule(CheckupSchedule);

            return RedirectToPage("./Index");
        }
    }
}
