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
using Microsoft.AspNetCore.Mvc.Rendering;
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
        
        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }

        [BindProperty]
        public IList<UserDto> Dentists { get; set; } = default!;
        public IList<Room> Rooms { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int dentisTId { get; set; }
        
        public string TimeRange { get; set; }

        public List<DentistSlotDto> DentistSlot { get; set; }

        public ServiceDto Service { get; set; } = default!;
        public async Task<IActionResult> OnGet(int id)
        {
            Appointment = await appointmentService.GetAppointmentByID(id);

            Service = await service.GetServiceByID(Appointment.ServiceId.Value);

            Rooms = roomService.GetAllActiveRooms().Result.Rooms;

            List<UserDto> dentist = new List<UserDto>();

            List<DentistSlotDto> dentistSlotDtos = new List<DentistSlotDto>();

            if (Appointment.CreateBy.HasValue)
            {
                dentist.Add(Appointment.CreateByNavigation);
                Dentists = dentist;
                dentistSlotDtos.Add(dentistSlotService.GetDentistSlotByAppointmentTimeStart(Appointment.TimeStart, Appointment.CreateBy.Value).DentistSlot);
                DentistSlot = dentistSlotDtos;
            }
            else
            {
                Dentists = await userService.GetAllDentistsByService((int)Appointment.ServiceId);
                dentistSlotDtos.Add(dentistSlotService
                    .GetDentistSlotByAppointmentTimeStart(Appointment.TimeStart, Dentists[0].UserId)
                    .DentistSlot);
                DentistSlot = dentistSlotDtos;
            }
            
            
            var date = Appointment.TimeStart;
            
            var morningStart = new TimeSpan(8, 0, 0);  
            var morningEnd = new TimeSpan(12, 0, 0);   
            
            var afternoonStart = new TimeSpan(13, 0, 0);  
            var afternoonEnd = new TimeSpan(17, 0, 0);   
            
            var eveningStart = new TimeSpan(17, 0, 0);  
            var eveningEnd = new TimeSpan(19, 30, 0);

            TimeOnly DentistSlotTimeStart = new TimeOnly();
            TimeOnly DentistSlotTimeEnd = new TimeOnly();
        
            if (date.TimeOfDay >= morningStart && date.TimeOfDay < morningEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(morningStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(morningEnd);
            }
            else if (date.TimeOfDay >= afternoonStart && date.TimeOfDay < afternoonEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(afternoonStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(afternoonEnd);
            }
            else if (date.TimeOfDay >= eveningStart && date.TimeOfDay < eveningEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(eveningStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(eveningEnd);
            }

            // Format the time range
            TimeRange = $"{DentistSlotTimeStart:hh\\:mm} - {DentistSlotTimeEnd:hh\\:mm}";

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

        public IActionResult OnGetGetDentistSchedule(int dentistId, DateTime timeStart)
        {
            HttpContext.Session.SetInt32("DentistId", dentistId);
            var dentistSlotResult = dentistSlotService.GetDentistSlotByAppointmentTimeStart(timeStart, dentistId);
    
            if (dentistSlotResult == null || dentistSlotResult.DentistSlot == null)
            {
                return new JsonResult(new { success = false, message = "No slot found" });
            }

            List<DentistSlotDto> dentistSlotDtos = new List<DentistSlotDto>();
            
            dentistSlotDtos.Add(dentistSlotResult.DentistSlot);

            var schedule = dentistSlotDtos.Select(d => new SelectListItem
            {
                Value = d.DentistSlotId.ToString(),
                Text =
                    $"{d.TimeStart.ToString("HH:mm")} - {d.TimeEnd.ToString("HH:mm")}"
            }).ToList();
            

            return new JsonResult(new { success = true, schedule });
        }

        public async Task<IActionResult> OnPostCreateDentistSlot()
        {
            var apppointmentId = (int)HttpContext.Session.GetInt32("AppointmentId");
            Appointment = await appointmentService.GetAppointmentByID(apppointmentId);
            var date = Appointment.TimeStart;
            
            var morningStart = new TimeSpan(8, 0, 0);  
            var morningEnd = new TimeSpan(12, 0, 0);   
            
            var afternoonStart = new TimeSpan(13, 0, 0);  
            var afternoonEnd = new TimeSpan(17, 0, 0);   
            
            var eveningStart = new TimeSpan(17, 0, 0);  
            var eveningEnd = new TimeSpan(19, 30, 0);

            TimeOnly DentistSlotTimeStart = new TimeOnly();

            TimeOnly DentistSlotTimeEnd = new TimeOnly();
            
            if (date.TimeOfDay >= morningStart && date.TimeOfDay < morningEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(morningStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(morningEnd);
            }else if (date.TimeOfDay >= afternoonStart && date.TimeOfDay < afternoonEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(afternoonStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(afternoonEnd);
            }else if (date.TimeOfDay >= eveningStart && date.TimeOfDay < eveningEnd)
            {
                DentistSlotTimeStart = TimeOnly.FromTimeSpan(eveningStart);
                DentistSlotTimeEnd = TimeOnly.FromTimeSpan(eveningEnd);
            }
            
            DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);

            DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
                DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);

            DentistSlotResult result = await dentistSlotService.CreateDentistSlot(dentisTId
                , slotTimeStart, slotTimeEnd, RoomId);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorProcessingAppointment_DentistSlot"] = result.Message;
                return RedirectToPage(new { id = Appointment.AppointmentId });
            }

            TempData["SuccessProcessingAppointment_DentistSlot"] = "Create dentist slot successfully!";
            return RedirectToPage(new { id = Appointment.AppointmentId });
        }
        public async Task<IActionResult> OnPostDelete(int appointmentId)
        {
            if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Reason and Customer Name are required.";
                return RedirectToPage(new {id = appointmentId});
            }
            AppointmentDto oldAppointment = await appointmentService.GetAppointmentByID(appointmentId);
            AppointmentResult result = appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, Reason);
            await hubContext.Clients.All.SendAsync("ReloadAppointments");
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteAppointment"] = result.Message;
                return RedirectToPage(new {id = appointmentId});
            }
            var receiver = oldAppointment.Customer.Email;
            var subject = "Your appointment has been denied!";
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "SorryForDenied.html");
            string body = await System.IO.File.ReadAllTextAsync(templatePath);
            body = body.Replace("[Service Details]", oldAppointment.Service.ServiceName)
                .Replace("[Date]", DateOnly.FromDateTime(oldAppointment.TimeStart).ToString())
                .Replace("[Time]", TimeOnly.FromDateTime(oldAppointment.TimeStart).ToString())
                .Replace("[Reason for Denial]", Reason);

            await emailSender.SendEmailAsync(receiver, subject, body);

            TempData["SuccessDeleteAppointment"] = "Delete successfully!";
            return RedirectToPage("/StaffPages/ProcessingAppointmentList");
        }
        


    }
}
