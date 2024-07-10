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
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.Medicines
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicineService _medicineService; 

        public DetailsModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public MedicineDto Medicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine =  await _medicineService.GetById(id);
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
    }
}
