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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class CreateModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public CreateModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public IActionResult OnGet()
        {
            // check role
            return Page();
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // add medicine
            _medicineService.CreateMedicine(Medicine);
            // signalR real-time

            return RedirectToPage("./Index");
        }
    }
}
