using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class CreateModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IMedicalRecordService medicalRecordService, IUserService userService, IHubContext<SignalRHub> hubContext)
        {
            _medicalRecordService = medicalRecordService;
            _userService = userService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_userService.GetAllCustomers().Result, "UserId", "Name");
            return Page();
        }

        [BindProperty]
        public MedicalRecordDto MedicalRecord { get; set; } = default!;
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            MedicalRecord.TimeStart = DateTime.Now;
            MedicalRecord.Duration = TimeOnly.FromDateTime(MedicalRecord.TimeStart);
            MedicalRecord.Status = true;
            await _medicalRecordService.CreateMedicalRecord(MedicalRecord);
            await _hubContext.Clients.All.SendAsync("ReloadMedicalRecords");

            return RedirectToPage("./Index");
        }
    }
}
