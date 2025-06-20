﻿using System;
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
using Service.Impl;

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
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
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

            try
            {
                Service.Status = true;
                await _service.CreateService(Service);
                await _hubContext.Clients.All.SendAsync("ReloadServices");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
