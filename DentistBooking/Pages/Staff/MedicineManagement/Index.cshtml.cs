using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Impl;

namespace DentistBooking.Pages.Staff.MedicineManagement
{
    public class IndexModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public IndexModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public IList<Medicine> Medicine { get;set; } = default!;

        public  async Task OnGetAsync()
        {
            Medicine =  _medicineService.GetAllMedicines();
        }
    }
}
