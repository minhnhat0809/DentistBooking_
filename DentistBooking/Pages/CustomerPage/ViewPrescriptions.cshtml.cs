using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service;

namespace DentistBooking.Pages.CustomerPage;

public class ViewPrescriptions : PageModel
{
    private readonly IPrescriptionService prescriptionService;

    public ViewPrescriptions(IPrescriptionService prescriptionService)
    {
        this.prescriptionService = prescriptionService;
    }
    public IList<PrescriptionDto> Prescriptions { get; set; }
    public async void OnGet()
    {
        Prescriptions = await prescriptionService.GetAllPrescriptionByCustomer
            (Int32.Parse(HttpContext.Session.GetString("ID")));
    }
}