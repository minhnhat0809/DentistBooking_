﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using X.PagedList;
using BusinessObject.DTO;
using Service.Impl;

namespace DentistBooking.Pages.AdminPage.Services
{
    public class IndexModel : PageModel
    {
        private readonly IService _service;
        public IndexModel(IService service)
        {
            _service = service;
        }

        public IPagedList<ServiceDto> Service { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
            var services = await _service.GetAllServices();
            Service = services.ToPagedList(PageNumber, PageSize);
            return Page();
        }
        
    }
}
