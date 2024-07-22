using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Service;
using Service.Impl;
using Service.Lib;

namespace DentistBooking.Pages.StaffPages
{
    public class ProcessingAppointmentListModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IHubContext<SignalRHub> hubContext;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        public ProcessingAppointmentListModel(IAppointmentService appointmentService, IConfiguration configuration, IEmailSender emailSender, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.hubContext = hubContext;
        }

        [BindProperty]
        public IList<AppointmentDto> Appointments { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }
        public async void OnGet()
        {
            Appointments = appointmentService.GetAllProcessingAppointment().Result;
        }

        public async Task<IActionResult> OnPostDelete(int appointmentId)
        {
            if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Reason and Customer Name are required.";
                return RedirectToPage("/StaffPages/ProcessingAppointmentList");
            }
            AppointmentDto oldAppointment = await appointmentService.GetAppointmentByID(appointmentId);
            AppointmentResult result = appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, Reason);
            await hubContext.Clients.All.SendAsync("ReloadAppointments");
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteAppointment"] = result.Message;
                Appointments = appointmentService.GetAllProcessingAppointment().Result;
                return Page();
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
            return RedirectToPage();
        }
    }
}
