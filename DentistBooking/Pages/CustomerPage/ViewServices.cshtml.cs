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
using X.PagedList;
using BusinessObject.DTO;
using Service.Impl;

namespace DentistBooking.Pages.CustomerPage
{
    public class InitialPageModel : PageModel
    {
        private readonly IService service;

        public InitialPageModel(IService service)
        {
            this.service = service;
        }

        public IPagedList<ServiceDto> Service { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync()
        {
            var services = await service.GetAllServices();
            Service = services.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
