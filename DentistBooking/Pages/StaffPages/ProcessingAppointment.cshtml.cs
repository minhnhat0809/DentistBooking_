using System.Collections;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Impl;
using System.Text.Json.Serialization;
using System.Text.Json;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.AspNetCore.SignalR;
using Service.Lib;

namespace DentistBooking.Pages.StaffPages
{
    public class ProcessingAppointmentModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;
        private readonly IService service;
        private readonly IUserService userService;
        private readonly IDentistSlotService dentistSlotService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly IHubContext<SignalRHub> hubContext;


        private readonly IRoomService roomService;
        public ProcessingAppointmentModel(IAppointmentService appointmentService, IDentistService dentistService
            , IService service, IUserService userService, IDentistSlotService dentistSlotService, IRoomService roomService
            , IEmailSender emailSender, IConfiguration configuration, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;
            this.dentistService = dentistService;
            this.service = service;
            this.userService = userService;
            this.dentistSlotService = dentistSlotService;
            this.roomService = roomService;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.hubContext = hubContext;
        }

        [BindProperty(SupportsGet = true)]
        public AppointmentDto Appointment { get; set; } = default!;

        [BindProperty]
        public IList<UserDto> Dentists { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeStart { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public TimeOnly DentistSlotTimeEnd { get; set; } = default!;

        public IList<Room> Rooms { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }

        public ServiceDto Service { get; set; } = default!;
        public async Task<IActionResult> OnGet(int id)
        {
            Appointment = await appointmentService.GetAppointmentByID(id);

            Service = await service.GetServiceByID(Appointment.ServiceId.Value);

            Rooms = roomService.GetAllActiveRooms().Result.Rooms;

            Dentists = await userService.GetAllDentistsByService((int)Appointment.ServiceId);



            HttpContext.Session.SetInt32("AppointmentId", Appointment.AppointmentId);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!Appointment.DentistSlotId.HasValue)
            {
                TempData["ErrorProcessingAppointment"] = "Please choose dentist slot!";
                return RedirectToPage(new { id = Appointment.AppointmentId });
            }
            AppointmentResult result = await appointmentService.UpdateAppointmentForStaff((int)Appointment.ServiceId,
                Appointment.AppointmentId, Appointment.TimeStart, Appointment.TimeEnd, (int)Appointment.DentistSlotId);

            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorProcessingAppointment"] = result.Message;
                return RedirectToPage(new { id = Appointment.AppointmentId });
            }
            hubContext.Clients.All.SendAsync("ReloadAppointments");
            var email = HttpContext.Session.GetString("Email");
            if (email != null)
            {
                var receiver = email;
                var subject = "Thank you for your booking!";
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "AcceptBooking.html");
                string body = await System.IO.File.ReadAllTextAsync(templatePath);
                body = body
                           .Replace("[Date]", DateOnly.FromDateTime(Appointment.TimeStart).ToString())
                           .Replace("[Time]", TimeOnly.FromDateTime(Appointment.TimeStart).ToString());

                await emailSender.SendEmailAsync(receiver, subject, body);
            }
            TempData["SuccessProcessingAppointmentError"] = "Appointment updated successfully!";
            return RedirectToPage(new { id = Appointment.AppointmentId });
        }

        public IActionResult OnGetDentistSchedule(int dentistId, DateTime timeStart)
        {
            HttpContext.Session.SetInt32("DentistId", dentistId);
            var dentistSlot = dentistSlotService.GetAllDentistSlotsByDentistAndDate(dentistId, DateOnly.FromDateTime(timeStart)).Result;
            var schedule = dentistSlot.Select(d => new
            {
                Id = d.DentistSlotId,
                TimeStart = d.TimeStart,
                TimeEnd = d.TimeEnd,
                Appointments = d.Appointments.Where(ap => !(ap.Status.Equals("Delete"))).ToList()
                }
            ).ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(schedule, options);
        }

        public async Task<IActionResult> OnPostCreateDentistSlot()
        {
            var apppointmentId = (int)HttpContext.Session.GetInt32("AppointmentId");
            Appointment = await appointmentService.GetAppointmentByID(apppointmentId);
            var date = Appointment.TimeStart;
            DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);

            DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);

            DentistSlotResult result = await dentistSlotService.CreateDentistSlot((int)HttpContext.Session.GetInt32("DentistId")
                , slotTimeStart, slotTimeEnd, RoomId);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorProcessingAppointment_DentistSlot"] = result.Message;
                return RedirectToPage(new { id = Appointment.AppointmentId });
            }

            TempData["SuccessProcessingAppointment_DentistSlot"] = "Create dentist slot successfully!";
            return RedirectToPage(new { id = Appointment.AppointmentId });
        }


    }
}
