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
using Microsoft.AspNetCore.SignalR;

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class CreateModel : PageModel
    {
        private readonly ICheckupScheduleService _checkupScheduleService; 
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IDentistService _dentistService;

        public CreateModel(ICheckupScheduleService checkupScheduleService, 
            IUserService userService, IDentistService dentistService,
            IHubContext<SignalRHub> hubContext)
        {
            _checkupScheduleService = checkupScheduleService;
            _userService = userService;
            _dentistService = dentistService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            CheckupSchedule = new CheckupSchedule()
            {
                TimeStart = DateTime.Now,
            };
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
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
                ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
                ViewData["DentistId"] = new SelectList(_userService.GetAllDentists().Result, "UserId", "Name");
                return Page();
            }
            CheckupSchedule.Status = true;
            await _checkupScheduleService.CreateCheckupSchedule(CheckupSchedule);
            await _hubContext.Clients.All.SendAsync("ReloadCheckupSchedules");

            return RedirectToPage("./Index");
        }
    }
}
