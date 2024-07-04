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

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class IndexModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public IndexModel(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        public IPagedList<Clinic> Clinic { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var clinics = await _clinicService.GetAllClinics();
            Clinic = clinics.ToPagedList(PageNumber, PageSize);
            return Page();
        }
        
    }
}
