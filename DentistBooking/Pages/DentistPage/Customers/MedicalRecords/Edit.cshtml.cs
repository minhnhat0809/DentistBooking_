using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
using Service;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Customers.MedicalRecords
{
    public class EditModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IMedicalRecordService medicalRecordService, IUserService userService, IHubContext<SignalRHub> hubContext)
        {
            _medicalRecordService = medicalRecordService;
            _userService = userService;
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
            MedicalRecord = medicalrecord;
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
                return Page();
            }


            try
            {
                _medicalRecordService.UpdateMedicalRecord(MedicalRecord);
                await _hubContext.Clients.All.SendAsync("ReloadMedicalRecords");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalRecordExists(MedicalRecord.MediaRecordId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./../Details", new { id = MedicalRecord.CustomerId });
        }

        private bool MedicalRecordExists(int id)
        {
            return _medicalRecordService.GetAllMedicalRecords().Result.Any(e => e.MediaRecordId == id);
        }
    }
}
