using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using BusinessObject.DTO;
using Service;

namespace DentistBooking.Pages.DentistPage.Customers.MedicalRecords
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
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _medicalRecordService.GetDtoById(id);
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
