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

        [BindProperty(SupportsGet = true)]
        public Appointment Appointment { get; set; } = default!;

        [BindProperty]
        public IList<User> Dentists { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeStart { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeEnd { get; set; } = default!;
        

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
            
            HttpContext.Session.SetInt32("AppointmentId",Appointment.AppointmentId);
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
             string result = appointmentService.UpdateAppointmentForStaff((int)Appointment.ServiceId,
                Appointment.AppointmentId, Appointment.TimeStart, Appointment.TimeEnd, (int)Appointment.DentistSlotId);
            if (!result.Equals("Success"))
            {
                TempData["ProcessingAppointmentError"] = result;
                Appointment = appointmentService.GetAppointmentByID(Appointment.AppointmentId);
                Services = service.GetAllServicesForCustomer((int)Appointment.ServiceId);
                Dentists = userService.GetAllDentistsByService((int)Appointment.ServiceId).Result;
                RedirectToPage(new { id = Appointment.AppointmentId });
            }

            TempData["ProcessingAppointment"] = "Appointment updated successfully!";
            Appointment = appointmentService.GetAppointmentByID(Appointment.AppointmentId);
            Services = service.GetAllServicesForCustomer((int)Appointment.ServiceId);
            Dentists = userService.GetAllDentistsByService((int)Appointment.ServiceId).Result;
            return RedirectToPage(new { id = Appointment.AppointmentId });
        }

        public IActionResult OnGetDentistByService(int id, int serviceId)
        {
            Dentists = userService.GetAllDentistsByService(serviceId).Result;
            var dentistList = Dentists.Select(d => new { userId = d.UserId, userName = d.Name }).ToList();
            return new JsonResult(dentistList);
        }

        public IActionResult OnGetDentistSchedule(int dentistId, DateTime timeStart)
        {
            HttpContext.Session.SetInt32("DentistId",dentistId);
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

        public IActionResult OnPostCreateDentistSlot()
        {
            var apppointmentId = (int)HttpContext.Session.GetInt32("AppointmentId");
            Appointment = appointmentService.GetAppointmentByID(apppointmentId);
            var date = Appointment.TimeStart;
            DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
            DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);
            
            string result = dentistSlotService.CreateDentistSlot((int)HttpContext.Session.GetInt32("DentistId")
                , slotTimeStart, slotTimeEnd);
            if (!result.Equals("Success"))
            {
                TempData["ProcessingAppointment_DentistSlot"] = result;
                return RedirectToPage(new { id = Appointment.AppointmentId });
            }

            TempData["ProcessingAppointment_DentistSlot"] = "Create dentist slot successfully!";
            return RedirectToPage(new { id = Appointment.AppointmentId });
        }


    }
}
