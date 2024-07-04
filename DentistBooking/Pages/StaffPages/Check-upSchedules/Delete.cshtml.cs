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

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class DeleteModel : PageModel
    {
        private readonly ICheckupScheduleService _checkupScheduleService;

        public DeleteModel(ICheckupScheduleService checkupScheduleService)
        {
            _checkupScheduleService = checkupScheduleService;   
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
            else
            {
                CheckupSchedule = checkupschedule;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkupschedule = await _checkupScheduleService.GetById(id);
            if (checkupschedule != null)
            {
                CheckupSchedule = checkupschedule;
                _checkupScheduleService.DeleteCheckupSchedule(checkupschedule.ScheduleId);
            }

            return RedirectToPage("./Index");
        }
    }
}
