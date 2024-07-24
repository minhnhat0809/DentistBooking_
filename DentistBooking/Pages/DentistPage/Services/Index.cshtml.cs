using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using BusinessObject.DTO;
using Service;
using X.PagedList;
using Service.Impl;

namespace DentistBooking.Pages.DentistPage.Services
{
    public class IndexModel : PageModel
    {
        private readonly IService _service;
        private readonly IUserService _userService;
        public IndexModel(IService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        public IPagedList<BusinessObject.DTO.ServiceDto> Service { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            
            string? email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(email))
            {
                var dentists = await _userService.GetAllDentists();
                if (dentists != null)
                {
                    var dentist = dentists.FirstOrDefault(x => x.Email == email);
                    if (dentist == null)
                    {
                        TempData["ErrorMessage"] = "No dentist found with the provided email.";
                        return RedirectToPage("/Error");
                    }
                    var services = await _service.GetAllServicesByDentistId(dentist.UserId);
                    Service = services.ToPagedList(PageNumber, PageSize);
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = "No dentist found with the provided email.";
                    return RedirectToPage("/Error");
                }
            }
            else return RedirectToPage("/Denied");
        }
    }
}
