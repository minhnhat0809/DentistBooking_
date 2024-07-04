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

namespace DentistBooking.Pages.AdminPage.Services
{
    public class DeleteModel : PageModel
    {
        private readonly IService _service;

        public DeleteModel(IService service)
        {
            _service = service;
        }

        [BindProperty]
        public BusinessObject.Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = _service.GetServiceByID(id);

            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = service;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = _service.GetServiceByID(id);
            if (service != null)
            {
                Service = service;
                _service.DeleteService(service);

            }

            return RedirectToPage("./Index");
        }
    }
}
