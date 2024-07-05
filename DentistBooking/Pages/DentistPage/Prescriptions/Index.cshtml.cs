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

namespace DentistBooking.Pages.DentistPage.Prescriptions
{
    public class IndexModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;

        public IndexModel(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public IPagedList<Prescription> Prescription { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            
            var prescriptions = _prescriptionService.GetPrescriptions();
            Prescription = prescriptions.ToPagedList(PageNumber, PageSize);

            return Page();
        }
    }
}
