using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.AdminPage.Services
{
    public class CreateModel : PageModel
    {
        private readonly IService _service;
        private readonly IHubContext<SignalRHub> _hubContext;
        public CreateModel(IService service, IHubContext<SignalRHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;   
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ServiceDto Service { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _service.CreateService(Service);
            await _hubContext.Clients.All.SendAsync("ReloadServices");
            return RedirectToPage("./Index");
        }
    }
}
