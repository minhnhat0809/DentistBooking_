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

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class EditModel : PageModel
    {
        private readonly ICheckupScheduleService  _checkupScheduleService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public EditModel(ICheckupScheduleService checkupScheduleService, IUserService userService, IHubContext<SignalRHub> hubContext)
        {
            _checkupScheduleService = checkupScheduleService;
            _userService = userService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public CheckupSchedule CheckupSchedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkupschedule = await _checkupScheduleService.GetById(id);
            if (checkupschedule == null)
            {
                return NotFound();
            }
            CheckupSchedule = checkupschedule;
           ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
           ViewData["DentistId"] = new SelectList(_userService.GetAllDentists().Result, "UserId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                _checkupScheduleService.UpdateCheckupSchedule(CheckupSchedule);
                await _hubContext.Clients.All.SendAsync("ReloadCheckupSchedules");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckupScheduleExists(CheckupSchedule.ScheduleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CheckupScheduleExists(int id)
        {
            return _checkupScheduleService.GetAllCheckupSchedules().Result.Any(e => e.ScheduleId == id);
        }
    }
}
