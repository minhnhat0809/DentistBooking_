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
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(IMedicalRecordService medicalRecordService, IHubContext<SignalRHub> hubContext)
        { 
            _medicalRecordService = medicalRecordService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public MedicalRecordDto MedicalRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _medicalRecordService.GetById(id);
            if (medicalrecord != null)
            {
                MedicalRecord = medicalrecord;
                await _medicalRecordService.DeleteMedicalRecord(MedicalRecord.MediaRecordId);
                await _hubContext.Clients.All.SendAsync("ReloadMedicalRecords");
            }

            return RedirectToPage("./Index");
        }
    }
}
