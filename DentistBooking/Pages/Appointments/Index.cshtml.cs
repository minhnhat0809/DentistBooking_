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

namespace DentistBooking.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public IList<Appointment> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Appointment = await appointmentService.GetAllAppointments();
        }
    }
}
