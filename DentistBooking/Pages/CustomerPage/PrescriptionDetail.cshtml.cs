using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Impl;

namespace DentistBooking.Pages.CustomerPage;

public class PrescriptionDetail : PageModel
{
    private readonly IPrescriptionMedicinesService prescriptionMedicinesService;

    public PrescriptionDetail(IPrescriptionMedicinesService prescriptionMedicinesService)
    {
        this.prescriptionMedicinesService = prescriptionMedicinesService;
    }

    public IList<PrescriptionMedicine> PrescriptionMedicines { get; set; } = default!;
    public void OnGet(int prescriptionId)
    {
        PrescriptionMedicines = prescriptionMedicinesService
            .GetAllPrescriptionMedicinesByPrescriptionId(prescriptionId).Result;
    }
}