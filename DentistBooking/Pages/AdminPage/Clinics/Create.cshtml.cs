using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Repository;
using Service;
using BusinessObject.DTO;
using Microsoft.AspNetCore.SignalR;

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class CreateModel : PageModel
    {
        private readonly IClinicService _clinicService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public CreateModel(IClinicService clinicService, IHubContext<SignalRHub> hubContext)
        {
            _clinicService = clinicService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ClinicDto Clinic { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Clinic.Status = true;
            _clinicService.CreateClinic(Clinic);
            await _hubContext.Clients.All.SendAsync("ReloadClinics");
            return RedirectToPage("./Index");
        }
    }
}
