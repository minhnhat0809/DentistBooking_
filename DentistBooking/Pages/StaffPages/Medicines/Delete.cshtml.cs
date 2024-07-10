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

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicineService _medicineService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(IMedicineService medicineService, IHubContext<SignalRHub> hubContext)
        {
            _medicineService = medicineService; 
            _hubContext = hubContext;
        }

        [BindProperty]
        public MedicineDto Medicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _medicineService.GetById(id);

            if (medicine == null)
            {
                return NotFound();
            }
            else
            {
                Medicine = medicine;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _medicineService.GetById(id);
            if (medicine != null)
            {
                Medicine = medicine;
                _medicineService.DeleteMedicine(Medicine.MedicineId);
                // signalR real-time

                await _hubContext.Clients.All.SendAsync("ReloadMedicines");
            }

            return RedirectToPage("./Index");
        }
    }
}
