using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service.Impl;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Appointments.Prescriptions.PrescriptionMedicines
{
    public class DetailsModel : PageModel
    {
        private readonly IPrescriptionMedicinesService _prescriptionMedicinesService;

        public DetailsModel(IPrescriptionMedicinesService prescriptionMedicinesService)
        {
            _prescriptionMedicinesService = prescriptionMedicinesService;
        }

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
            else
            {
                PrescriptionMedicine = prescriptionmedicine;
            }
            return Page();
        }
    }
}
