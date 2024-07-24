using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Runtime.CompilerServices;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Lib;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DentistBooking.Pages.CustomerPage
{
    public class BookModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IService service;
        private readonly IEmailSender emailSender;
        private readonly IUserService _userService;
        private readonly IConfiguration configuration;
        private readonly IHubContext<SignalRHub> hubContext;

        public BookModel(IAppointmentService appointmentService, IService service, IUserService _userService, IEmailSender emailSender, IConfiguration configuration, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;
            this.service = service;
            this.emailSender = emailSender;
            this._userService = _userService;
            this.hubContext = hubContext;
            this.configuration = configuration;

        }
        [BindProperty]
        public DateOnly selectedDate { get; set; }
        
        [BindProperty]
        [Required]
        public int SelectedServiceId { get; set; }


        [BindProperty]
        public DateTime startTime { get; set; }

        [BindProperty] 
        public int SelectedDentistId { get; set; }

        public IList<ServiceDto> Services { get; set; } = default!;
        
        public List<UserDto> Dentists { get; set; } = new List<UserDto>();


        public async void OnGet(int id)
        {
            SelectedServiceId = id;
            var services = (await service.GetAllActiveServices()).Services;

            foreach (var s in services)
            {
                if (s.ServiceId == id)
                {
                    var c = s;
                    services.Remove(s);
                    services.Insert(0,c);
                    break;
                }
            }

            Services = services;
            
            Dentists = await _userService.GetAllDentistsByService(SelectedServiceId);
        }

        public async Task<IActionResult> OnPostBookAsync()
        {
            var customerId = Int32.Parse(HttpContext.Session.GetString("ID"));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Dictionary<string, string> result = await appointmentService.CreateAppointmentWithDentist(startTime, customerId, selectedDate, SelectedServiceId, SelectedDentistId);
            var services = (await service.GetAllActiveServices()).Services;
            if (!result.ContainsKey("Success"))
            {
                foreach (var item in result)
                {
                    TempData["Book"] = item.Value;
                }
               
                Services = services;
                Dentists = await _userService.GetAllDentistsByService(SelectedServiceId);
                return Page();
            }
            TempData["Book"] = "Appointment created successfully!";
            Services = services;
            Dentists = await _userService.GetAllDentistsByService(SelectedServiceId);
            hubContext.Clients.All.SendAsync("ReloadAppointments");
            var email = HttpContext.Session.GetString("Email");
            if (email != null)
            {
                var receiver = email;
                var subject = "Thank you for your booking!";
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "ThanksCustomerBooking.html");
                string body = await System.IO.File.ReadAllTextAsync(templatePath);
                var selectedService = await service.GetServiceByID(SelectedServiceId);
                body = body.Replace("[Service Details]", selectedService.ServiceName)
                           .Replace("[Date]", DateOnly.FromDateTime(startTime).ToString())
                           .Replace("[Time]", TimeOnly.FromDateTime(startTime).ToString());

                await emailSender.SendEmailAsync(receiver, subject, body);
            }

            return RedirectToPage(new { id = SelectedServiceId });
        }
        
        public async Task<IActionResult> OnGetDentistsByServiceAsync(int serviceId)
        {
            try
            {
                var dentists = await _userService.GetAllDentistsByService(serviceId);
                if (dentists == null || !dentists.Any())
                {
                    return new JsonResult(new { success = false, message = "No dentists available for this service" });
                }

                var newDentists = dentists.Select(dentist => new SelectListItem
                {
                    Value = dentist.UserId.ToString(),
                    Text = dentist.Name
                }).ToList();

                return new JsonResult(new { success = true, newDentists });
            }
            catch (Exception ex)
            {
                // Log the exception
                return new JsonResult(new { success = false, message = "An error occurred while retrieving dentists." });
            }
        }
    }
}
