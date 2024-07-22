using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Runtime.CompilerServices;
using BusinessObject.DTO;
using Service.Lib;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DentistBooking.Pages.CustomerPage
{
    public class BookModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IService service;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly IHubContext<SignalRHub> hubContext;

        public BookModel(IAppointmentService appointmentService, IService service, IEmailSender emailSender, IConfiguration configuration, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;
            this.service = service;
            this.emailSender = emailSender;
            this.hubContext = hubContext;
            this.configuration = configuration;

        }
        [BindProperty]
        public DateOnly selectedDate { get; set; }


        [BindProperty]
        public DateTime startTime { get; set; }

        [BindProperty]
        public int serviceId { get; set; }

        public IList<ServiceDto> Services { get; set; } = default!;


        public async void OnGet(int id)
        {
            serviceId = id;
            Services = await service.GetAllServicesForCustomer(serviceId);
        }

        public async Task<IActionResult> OnPostBookAsync()
        {
            var customerId = Int32.Parse(HttpContext.Session.GetString("ID"));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Dictionary<string, string> result = await appointmentService.CreateAppointment(startTime, customerId, selectedDate, serviceId);

            if (!result.ContainsKey("Success"))
            {
                foreach (var item in result)
                {
                    TempData["Book"] = item.Value;
                }
                return Page();
            }
            TempData["Book"] = "Appointment created successfully!";
            hubContext.Clients.All.SendAsync("ReloadAppointments");
            var email = HttpContext.Session.GetString("Email");
            if (email != null)
            {
                var receiver = email;
                var subject = "Thank you for your booking!";
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "ThanksCustomerBooking.html");
                string body = await System.IO.File.ReadAllTextAsync(templatePath);
                var selectedService = await service.GetServiceByID(serviceId);
                body = body.Replace("[Service Details]", selectedService.ServiceName)
                           .Replace("[Date]", DateOnly.FromDateTime(startTime).ToString())
                           .Replace("[Time]", TimeOnly.FromDateTime(startTime).ToString());

                await emailSender.SendEmailAsync(receiver, subject, body);
            }

            return RedirectToPage(new { id = serviceId });
        }
    }
}
