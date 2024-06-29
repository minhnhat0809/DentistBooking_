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

namespace DentistBooking.Pages.CustomerPage
{
    public class InitialPageModel : PageModel
    {
        private readonly IService service;

        public InitialPageModel(IService service)
        {
            this.service = service;
        }

        public IList<BusinessObject.Service> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Service = service.GetAllServices();
        }
    }
}
