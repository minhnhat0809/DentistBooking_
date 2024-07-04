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

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public IndexModel(IMedicalRecordService medicalRecordService)
        { 
            _medicalRecordService = medicalRecordService;
        }

        public IPagedList<MedicalRecordDto> MedicalRecord { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            var medicalRecords = _medicalRecordService.GetAllMedicalRecords();
            MedicalRecord = medicalRecords.Result.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
