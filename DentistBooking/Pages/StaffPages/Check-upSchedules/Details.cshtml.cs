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

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class DetailsModel : PageModel
    {
        private readonly ICheckupScheduleService _checkupScheduleService;

        public DetailsModel(ICheckupScheduleService checkupScheduleService)
        {
            _checkupScheduleService = checkupScheduleService;
        }

        public CheckupScheduleDto CheckupSchedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkupschedule = await _checkupScheduleService.GetDtoById(id);
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
    }
}
