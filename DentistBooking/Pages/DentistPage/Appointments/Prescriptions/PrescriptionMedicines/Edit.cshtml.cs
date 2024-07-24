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

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions.PrescriptionMedicines
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
        public SelectList MedicineSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Dentist")
            {
                return RedirectToPage("/Denied");
            }
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
                ViewData["MedicineId"] = new SelectList(await _medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
                ViewData["PrescriptionId"] = new SelectList(await _prescriptionService.GetPrescriptions(), "PrescriptionId", "PrescriptionId");
                return Page();
            }

            try
            {
                await _prescriptionMedicinesService.UpdatePrescriptionMedicine(PrescriptionMedicine);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");
                await _prescriptionService.UpdatePrescriptionPrice(PrescriptionMedicine.PrescriptionId.Value);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PrescriptionMedicineExists(PrescriptionMedicine.PrescriptionMedicineId))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            ViewData["MedicineId"] = new SelectList(await _medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
            ViewData["PrescriptionId"] = new SelectList(await _prescriptionService.GetPrescriptions(), "PrescriptionId", "PrescriptionId");

            return RedirectToPage("./Index", new { id = PrescriptionMedicine.PrescriptionId });
        }

        private bool PrescriptionMedicineExists(int id)
        {
            return _prescriptionMedicinesService.GetAllPrescriptionMedicines().Result.Any(e => e.PrescriptionMedicineId == id);
        }

    }
}
