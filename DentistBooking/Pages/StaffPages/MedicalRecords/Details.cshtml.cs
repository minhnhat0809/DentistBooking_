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

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public DetailsModel(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public MedicalRecordDto MedicalRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Staff")
            {
                return RedirectToPage("/Denied");
            }
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _medicalRecordService.GetById(id);
            if (medicalrecord == null)
            {
                return NotFound();
            }
            else
            {
                MedicalRecord = medicalrecord;
            }
            return Page();
        }
    }
}
