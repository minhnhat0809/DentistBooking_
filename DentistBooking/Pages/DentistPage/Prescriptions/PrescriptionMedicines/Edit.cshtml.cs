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
using Service.Impl;
using Microsoft.AspNetCore.SignalR;
using Service;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Prescriptions.PrescriptionMedicines
{
    public class EditModel : PageModel
    {
        private readonly IPrescriptionMedicinesService _prescriptionMedicinesService;
        private readonly IMedicineService _medicineService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IPrescriptionMedicinesService prescriptionMedicinesService, 
            IHubContext<SignalRHub> hubContext,
            IPrescriptionService prescriptionService,
            IMedicineService medicineService)
        {
            _hubContext = hubContext;
            _prescriptionMedicinesService = prescriptionMedicinesService;   
            _medicineService = medicineService;
            _prescriptionService = prescriptionService;
        }

        [BindProperty]
        public PrescriptionMedicineDto PrescriptionMedicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionmedicine = await _prescriptionMedicinesService.GetById(id.Value);
            if (prescriptionmedicine == null)
            {
                return NotFound();
            }
            PrescriptionMedicine = prescriptionmedicine;
            ViewData["MedicineId"] = new SelectList(await _medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
            ViewData["PrescriptionId"] = new SelectList(await _prescriptionService.GetPrescriptions(), "PrescriptionId", "PrescriptionId");
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
                _prescriptionMedicinesService.UpdatePrescriptionMedicine(PrescriptionMedicine);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionMedicineExists(PrescriptionMedicine.PrescriptionMedicineId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var id = PrescriptionMedicine.PrescriptionId;
            return RedirectToPage("./Index", new { id = PrescriptionMedicine.PrescriptionId });
        }

        private bool PrescriptionMedicineExists(int id)
        {
            return _prescriptionMedicinesService.GetAllPrescriptionMedicines().Result.Any(e => e.PrescriptionMedicineId == id);
        }
    }
}
