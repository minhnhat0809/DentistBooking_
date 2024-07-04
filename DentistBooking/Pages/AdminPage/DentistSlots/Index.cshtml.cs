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
using Service.Impl;

namespace DentistBooking.Pages.AdminPage.DentistSlots
{
    public class IndexModel : PageModel
    {
        private readonly IDentistSlotService _slotService;

        public IndexModel(IDentistSlotService slotService)
        {
            _slotService = slotService; 
        }

        public IPagedList<DentistSlot> DentistSlot { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var slots = await _slotService.GetAllDentistSlots();
            DentistSlot = slots.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
