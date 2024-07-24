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
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;
using Service.Impl;

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class EditModel : PageModel
    {
        private readonly IMedicineService _medicineService; 
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IMedicineService medicineService, IHubContext<SignalRHub> hubContext)
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
            Medicine = medicine;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _medicineService.UpdateMedicine(Medicine);
                await _hubContext.Clients.All.SendAsync("ReloadMedicines");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await MedicineExists(Medicine.MedicineId))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
        private async Task<bool> MedicineExists(int id)
        {
            // Ensure that the service exists by querying the database
            var services = await _medicineService.GetAllMedicines();
            return services.Any(e => e.MedicineId == id);
        }
    }
}
