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
using X.PagedList;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.Check_upSchedules
{
    public class IndexModel : PageModel
    {
        private readonly ICheckupScheduleService _checkupScheduleService;

        public IndexModel(ICheckupScheduleService checkupScheduleService)
        {
            _checkupScheduleService = checkupScheduleService;
        }


        public IPagedList<CheckupScheduleDto> CheckupSchedule { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var checkupSchedules = await _checkupScheduleService.GetAllCheckupSchedules();
            CheckupSchedule = checkupSchedules.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
