using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Impl;
using X.PagedList;

namespace DentistBooking.Pages.CustomerPage;

public class PrescriptionDetail : PageModel
{
    private readonly IPrescriptionMedicinesService prescriptionMedicinesService;

    public PrescriptionDetail(IPrescriptionMedicinesService prescriptionMedicinesService)
    {
        this.prescriptionMedicinesService = prescriptionMedicinesService;
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

        try
        {
            var prescriptionMedicines = await prescriptionMedicinesService.GetAllPrescriptionMedicinesByPrescriptionId(id);
            prescriptionMedicines = prescriptionMedicines.Where(x=>x.Status == true).ToList();
            PrescriptionMedicine = prescriptionMedicines.ToPagedList(PageNumber, PageSize);

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while retrieving prescription medicines: " + ex.Message;
            return RedirectToPage("/Error");
        }
    }
}