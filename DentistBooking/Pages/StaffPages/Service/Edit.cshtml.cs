using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.Service
{
    public class EditModel : PageModel
    {
        private readonly IService _service;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IService service, IHubContext<SignalRHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ServiceDto Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _service.GetServiceByID(id);
            if (service == null)
            {
                return NotFound();
            }
            Service = service;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Service.Status = true;
                await _service.UpdateService(Service);
                await _hubContext.Clients.All.SendAsync("ReloadServices");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await ServiceExistsAsync(Service.ServiceId))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        // Async version of ServiceExists method
        private async Task<bool> ServiceExistsAsync(int id)
        {
            // Ensure that the service exists by querying the database
            var services = await _service.GetAllServices();
            return services.Any(e => e.ServiceId == id);
        }

    }
}
