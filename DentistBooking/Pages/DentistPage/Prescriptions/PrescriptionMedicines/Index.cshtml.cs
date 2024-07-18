using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Service;
using Service.Impl;
using X.PagedList;
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Prescriptions.PrescriptionMedicines
{
    public class IndexModel : PageModel
    {
        private readonly IPrescriptionMedicinesService _prescriptionMedicinesService;

        public IndexModel(IPrescriptionMedicinesService prescriptionMedicinesService)
        {
            _prescriptionMedicinesService = prescriptionMedicinesService;
        }

        public IPagedList<PrescriptionMedicineDto> PrescriptionMedicine { get; set; } = default!;
        public int PrescriptionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            PrescriptionId = id;    
            var prescriptionMedicines = await _prescriptionMedicinesService.GetAllPrescriptionMedicinesByPrescriptionId(id);
            if (prescriptionMedicines == null || !prescriptionMedicines.Any())
            {
                return NotFound();
            }
            PrescriptionMedicine = prescriptionMedicines.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
