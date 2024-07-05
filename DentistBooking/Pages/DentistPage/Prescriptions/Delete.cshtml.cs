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
using Microsoft.AspNetCore.SignalR;

namespace DentistBooking.Pages.DentistPage.Prescriptions
{
    public class DeleteModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(IPrescriptionService prescriptionService, IHubContext<SignalRHub> hubContext)
        {
            _prescriptionService = prescriptionService; 
            _hubContext = hubContext;   
        }

        [BindProperty]
        public Prescription Prescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = _prescriptionService.GetById(id.Value);

            if (prescription == null)
            {
                return NotFound();
            }
            else
            {
                Prescription = prescription;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = _prescriptionService.GetById(id.Value);
            if (prescription != null)
            {
                Prescription = prescription;
                _prescriptionService.DeletePrescription(Prescription.PrescriptionId);
                await _hubContext.Clients.All.SendAsync("ReloadPrescriptions");
            }

            return RedirectToPage("./Index");
        }
    }
}
