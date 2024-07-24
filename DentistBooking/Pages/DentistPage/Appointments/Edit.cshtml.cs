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
        private readonly IPrescriptionService _prescriptionService1;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(
            IAppointmentService appointmentService,
            IUserService userService,
            IDentistSlotService dentistSlotService,
            IMedicalRecordService medicalRecordService,
            IService serviceService,
            IPrescriptionService prescriptionService,
            IHubContext<SignalRHub> hubContext)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _dentistSlotService = dentistSlotService;
            _medicalRecordService = medicalRecordService;
            _serviceService = serviceService;
            _hubContext = hubContext;
            _prescriptionService1 = prescriptionService;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public List<PrescriptionDto> Prescriptions { get; set; } = default!;

        public SelectList StatusOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // check null
            if (id != null)
            {
                // check role
                /*var role = HttpContext.Session.GetString("Role");
                if (role != "Dentist")
                {
                    return RedirectToPage("/Denied");
                }*/
                try
                {
                    var appointment = await _appointmentService.GetAppointmentByID(id.Value);
                    // check exist
                    if (appointment != null)
                    {
                        // try get status
                        if (appointment.Status != null)
                        {
                            if (appointment.Status == "Happening" || appointment.Status == "Finished") StatusOptions = new SelectList(new List<string> { "Happening", "Finished" });
                            else StatusOptions = new SelectList(new List<string> { "Happening", "Finished", appointment.Status });
                        }

                        Appointment = appointment;

                        // try get prescription 
                        Prescriptions = await _prescriptionService1.GetByAppointmentId(appointment.AppointmentId);

                        await LoadSelectLists();
                        return Page();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Appointment not found.";
                        return RedirectToPage("/Error");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                    return RedirectToPage("/Error");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Appointment ID is missing.";
                return RedirectToPage("/Error");
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                StatusOptions = new SelectList(new List<string> { "Happening", "Finished", Appointment.Status });
                return Page();
            }

            try
            {
                await _appointmentService.PutAppointment(Appointment);
                await _hubContext.Clients.All.SendAsync("ReloadAppointments");
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AppointmentExists(Appointment.AppointmentId))
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToPage("/Error");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the appointment: " + ex.Message);
                    await LoadSelectLists();
                    StatusOptions = new SelectList(new List<string> { "Happening", "Finished", Appointment.Status });
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                await LoadSelectLists();
                StatusOptions = new SelectList(new List<string> { "Happening", "Finished", Appointment.Status });
                return Page();
            }
        }

        private bool AppointmentExists(int id)
        {
            try
            {
                return _appointmentService.GetAllAppointments().Result.Any(e => e.AppointmentId == id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while checking appointment existence: " + ex.Message;
                return false;
            }
        }


        private async Task LoadSelectLists()
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading select lists: " + ex.Message;
            }
        }
    }
}
