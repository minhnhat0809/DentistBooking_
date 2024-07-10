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
using Microsoft.AspNetCore.SignalR;

namespace DentistBooking.Pages.AdminPage.Clinics
{
    public class DeleteModel : PageModel
    {
        private readonly IClinicService _clinicService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public DeleteModel(IClinicService clinicService, IHubContext<SignalRHub> hubContext)
        {
            _clinicService = clinicService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ClinicDto Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _clinicService.GetById(id);

            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                Clinic = clinic;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _clinicService.GetById(id);
            if (clinic != null)
            {
                Clinic = clinic;
                _clinicService.DeleteClinic(Clinic.ClinicId);
                await _hubContext.Clients.All.SendAsync("ReloadClinics");

            }

            return RedirectToPage("./Index");
        }
    }
}
