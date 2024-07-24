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

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions
{
    public class IndexModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;

        public IndexModel(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public IPagedList<PrescriptionDto> Prescription { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var prescriptions = await _prescriptionService.GetPrescriptions();
                Prescription = prescriptions.ToPagedList(PageNumber, PageSize);

                return Page();
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToPage("/Error");
            }
        }
    }
}
