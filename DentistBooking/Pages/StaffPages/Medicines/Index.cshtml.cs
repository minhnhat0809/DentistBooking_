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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class IndexModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public IndexModel(IMedicineService medicineService )
        {
            _medicineService = medicineService;
        }

        public IPagedList<Medicine> Medicine { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var medicines = _medicineService.GetAllMedicines();
            Medicine = medicines.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
