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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class CreateModel : PageModel
    {
        private readonly IMedicineService _medicineService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IMedicineService medicineService, IHubContext<SignalRHub> hubContext)
        {
            _medicineService = medicineService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            // check role
            return Page();
        }

        [BindProperty]
        public MedicineDto Medicine { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // add medicine
            Medicine.Status = true;
            await _medicineService.CreateMedicine(Medicine);
            // signalR real-time
            await _hubContext.Clients.All.SendAsync("ReloadMedicines");
            return RedirectToPage("./Index");
        }
    }
}
