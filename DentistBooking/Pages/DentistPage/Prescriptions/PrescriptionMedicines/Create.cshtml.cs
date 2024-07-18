using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service.Impl;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Prescriptions.PrescriptionMedicines
{
    public class CreateModel : PageModel
    {
        private readonly IPrescriptionMedicinesService _prescriptionMedicinesService;
        private readonly IMedicineService _medicineService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IPrescriptionMedicinesService prescriptionMedicinesService,
            IPrescriptionService prescriptionService,
            IMedicineService medicineService,
            IHubContext<SignalRHub> hubContext)
        {
            _prescriptionMedicinesService = prescriptionMedicinesService;
            _prescriptionService = prescriptionService;
            _medicineService = medicineService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public PrescriptionMedicineDto PrescriptionMedicine { get; set; } = new PrescriptionMedicineDto();

        public SelectList PrescriptionIdSelectList { get; set; }
        public SelectList MedicineIdSelectList { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            // Initialize PrescriptionId and make it view-only
            PrescriptionMedicine.PrescriptionId = id;

            // Populate dropdowns
            PrescriptionIdSelectList = new SelectList(await _prescriptionService.GetPrescriptions(), "PrescriptionId", "PrescriptionId", id);
            MedicineIdSelectList = new SelectList(await _medicineService.GetAllMedicines(), "MedicineId", "MedicineName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PrescriptionIdSelectList = new SelectList(await _prescriptionService.GetPrescriptions(), "PrescriptionId", "PrescriptionId", PrescriptionMedicine.PrescriptionId);
                MedicineIdSelectList = new SelectList(await _medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
                return Page();
            }
            PrescriptionMedicine.Status = true;
            _prescriptionMedicinesService.AddPrescriptionMedicine(PrescriptionMedicine);
            await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");

            return RedirectToPage("./Index", new { id = PrescriptionMedicine.PrescriptionId });
        }
    }
}
