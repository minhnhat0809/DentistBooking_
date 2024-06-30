using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.CustomerPage
{
    public class ViewClinicModel : PageModel
    {
        private readonly IClinicService clinicService;
        public ViewClinicModel(IClinicService clinicService)
        {
            this.clinicService = clinicService;
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;
        public void OnGet()
        {
            Clinic = clinicService.GetById(1);
        }
    }
}
