﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service.Impl;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;
using Service;

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions.PrescriptionMedicines
{
    public class DeleteModel : PageModel
    {
        private readonly IPrescriptionMedicinesService _prescriptionMedicinesService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public DeleteModel(IPrescriptionMedicinesService prescriptionMedicinesService,
            IHubContext<SignalRHub> hubContext,
            IPrescriptionService prescriptionService)
        {
            _hubContext = hubContext;   
            _prescriptionMedicinesService = prescriptionMedicinesService;
            _prescriptionService = prescriptionService;
        }

        [BindProperty]
        public PrescriptionMedicineDto PrescriptionMedicine { get; set; } = default!;

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
            else
            {
                PrescriptionMedicine = prescriptionmedicine;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionmedicine = await _prescriptionMedicinesService.GetById(id.Value);
            if (prescriptionmedicine != null)
            {
                PrescriptionMedicine = prescriptionmedicine;
                await _prescriptionMedicinesService.DeletePrescriptionMedicine(prescriptionmedicine);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");
                await _prescriptionService.UpdatePrescriptionPrice(PrescriptionMedicine.PrescriptionId.Value);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptionMedicines");
            }

            return RedirectToPage("./Index", new { id = PrescriptionMedicine.PrescriptionId });
        }
    }
}
