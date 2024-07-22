using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Service;
using BusinessObject.DTO;
using X.PagedList;

namespace DentistBooking.Pages.DentistPage.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public IndexModel(IAppointmentService appointmentService, IUserService userService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
        }

        public IPagedList<AppointmentDto> Appointment { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task OnGetAsync()
        {
            string? email = HttpContext.Session.GetString("Email");
            var dentists = await _userService.GetAllDentists();
            var dentist = dentists.FirstOrDefault(x => x.Email == email);
            List<AppointmentDto> viewModels = await _appointmentService.GetAllAppointmentByDentistId(dentist.UserId);
            Appointment = viewModels.ToPagedList(PageNumber, PageSize); 
        }
    }
}
