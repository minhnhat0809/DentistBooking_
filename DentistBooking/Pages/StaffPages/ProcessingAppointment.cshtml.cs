using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Impl;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DentistBooking.Pages.StaffPages
{
    public class ProcessingAppointmentModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;
        private readonly IService service;
        private readonly IUserService userService;
        private readonly IDentistSlotService dentistSlotService;
        public ProcessingAppointmentModel(IAppointmentService appointmentService, IDentistService dentistService
            , IService service, IUserService userService, IDentistSlotService dentistSlotService)
        {
            this.appointmentService = appointmentService;
            this.dentistService = dentistService;
            this.service = service;
            this.userService = userService;
            this.dentistSlotService = dentistSlotService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        [BindProperty]
        public IList<User> Dentists { get; set; } = default!;

        public IList<BusinessObject.Service> Services { get; set; } = default!;
        public IActionResult OnGet(int id)
        {
            Appointment = appointmentService.GetAppointmentByID(id);
            if (Appointment.DentistSlot != null)
            {
                Services = dentistService.GetAllServiceByDentist((int)Appointment.DentistSlot.DentistId, (int)Appointment.ServiceId);
            }
            else
            {
                Services = service.GetAllServicesForCustomer((int)Appointment.ServiceId);
            }
            Dentists = userService.GetAllDentistsByService((int)Appointment.ServiceId).Result;

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
             string result = appointmentService.UpdateAppointmentForStaff((int)Appointment.ServiceId,
                Appointment.AppointmentId, Appointment.TimeStart, Appointment.TimeEnd, (int)Appointment.DentistSlotId);
            if (!result.Equals("Success"))
            {
                TempData["AppointmentDetail"] = result;
                Appointment = appointmentService.GetAppointmentByID(Appointment.AppointmentId);
                
                Services = service.GetAllServicesForCustomer((int)Appointment.ServiceId);
                return Page();
            }

            TempData["ProcessingAppointment"] = "Appointment updated successfully!";
            Appointment = appointmentService.GetAppointmentByID(Appointment.AppointmentId);
            Services = service.GetAllServicesForCustomer((int)Appointment.ServiceId);
            Dentists = userService.GetAllDentistsByService((int)Appointment.ServiceId).Result;
            return Page();
        }

        public IActionResult OnGetDentistByService(int id, int serviceId)
        {
            Dentists = userService.GetAllDentistsByService(serviceId).Result;
            var dentistList = Dentists.Select(d => new { userId = d.UserId, userName = d.UserName }).ToList();
            return new JsonResult(dentistList);
        }

        public IActionResult OnGetDentistSchedule(int dentistId, DateTime timeStart)
        {
            var dentistSlot = dentistSlotService.GetAllDentistSlotsByDentistAndDate(dentistId, DateOnly.FromDateTime(timeStart)).Result;
            var schedule = dentistSlot.Select(d => new { 
                Id = d.DentistSlotId,
                TimeStart = d.TimeStart,
                TimeEnd = d.TimeEnd,
                Appointments = d.Appointments
                }
            ).ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return new JsonResult(schedule, options);
        }


    }
}
