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

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class CreateModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;

        public CreateModel(IMedicalRecordService medicalRecordService, IUserService userService)
        {
            _medicalRecordService = medicalRecordService;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Name");
            return Page();
        }

        [BindProperty]
        public MedicalRecord MedicalRecord { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _medicalRecordService.CreateMedicalRecord(MedicalRecord);

            return RedirectToPage("./Index");
        }
    }
}
