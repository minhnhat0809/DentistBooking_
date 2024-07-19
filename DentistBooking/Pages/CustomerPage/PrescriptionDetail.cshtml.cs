using BusinessObject;
using BusinessObject.DTO;
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

    public IList<PrescriptionMedicineDto> PrescriptionMedicines { get; set; } = default!;
    public async void OnGet(int prescriptionId)
    {
        PrescriptionMedicines = await prescriptionMedicinesService
            .GetAllPrescriptionMedicinesByPrescriptionId(prescriptionId);
    }
}