using BusinessObject;
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
    public IList<Prescription> Prescriptions { get; set; }
    public void OnGet()
    {
        Prescriptions = prescriptionService.GetAllPrescriptionByCustomer
            (Int32.Parse(HttpContext.Session.GetString("ID")));
    }
}