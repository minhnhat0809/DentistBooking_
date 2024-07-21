using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Service;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Net.NetworkInformation;

namespace DentistBooking.Pages.DentistPage.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IDentistSlotService _dentistSlotService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IService _serviceService;

        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(
            IAppointmentService appointmentService,
            IUserService userService,
            IDentistSlotService dentistSlotService,
            IMedicalRecordService medicalRecordService,
            IService serviceService,
            IHubContext<SignalRHub> hubContext)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _medicalRecordService = medicalRecordService;
            _serviceService = serviceService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;
        public SelectList StatusOptions { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            List<string> status = await _appointmentService.GetAllStatusOfAppointment(appointment.AppointmentId);
            if (status == null)
            {
                return NotFound();
            }
            StatusOptions = new SelectList(status);
            Appointment = appointment;
            await LoadSelectLists();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return Page();
            }

            try
            {
                await _appointmentService.PutAppointment(Appointment);
                await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(Appointment.AppointmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AppointmentExists(int id)
        {
            return _appointmentService.GetAllAppointments().Result.Any(e => e.AppointmentId == id);
        }

        private async Task LoadSelectLists()
        {
            var users = await _userService.GetAllUsers();
            var dentistSlots = await _dentistSlotService.GetAllDentistSlots();
            var medicalRecords = await _medicalRecordService.GetAllMedicalRecords();
            var services = await _serviceService.GetAllServices();

            ViewData["CreateBy"] = new SelectList(users, "UserId", "Name");
            ViewData["CustomerId"] = new SelectList(users, "UserId", "Name");
            ViewData["DentistSlotId"] = new SelectList(dentistSlots, "DentistSlotId", "DentistSlotId");
            ViewData["MedicalRecordId"] = new SelectList(medicalRecords, "MediaRecordId", "MediaRecordId");
            ViewData["ModifiedBy"] = new SelectList(users, "UserId", "Name");
            ViewData["ServiceId"] = new SelectList(services, "ServiceId", "ServiceName");
        }
    }
}
