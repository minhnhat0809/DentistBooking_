using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using BusinessObject.DTO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BusinessObject;
using Service.Impl;
using Service.Lib;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentistBooking.Pages
{
    public class HomePageModel : PageModel
    {

        private readonly IAppointmentService appointmentService;
        private readonly IService service;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly IHubContext<SignalRHub> hubContext;
        public HomePageModel(IUserService userService, IAppointmentService appointmentService, IService service, 
            IEmailSender emailSender, IConfiguration configuration, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;
            this.service = service;
            this.userService = userService;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.hubContext = hubContext;
        }

        public List<ServiceDto> Services { get; set; } = new List<ServiceDto>();
        public List<UserDto> Dentists { get; set; } = new List<UserDto>();

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [BindProperty]
        [Required]
        public int SelectedServiceId { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentTime { get; set; }

        [BindProperty]
        [Required]
        public string Gender { get; set; }

        [BindProperty]
        public int SelectedDentistId
        { get; set; }
        public async Task OnGetAsync()
        {
            Services = (await service.GetAllActiveServices()).Services;
            Dentists = await userService.GetAllDentistsByService(SelectedServiceId);
        }
        public async Task<IActionResult> OnGetDentistsByServiceAsync(int serviceId)
        {
            try
            {
                var dentists = await userService.GetAllDentistsByService(serviceId);
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


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Services = (await service.GetAllActiveServices()).Services;
                Dentists = await userService.GetAllDentistsByService(SelectedServiceId);
                return Page();
            }

            var customer = await userService.GetCustomerByPhoneNumber(PhoneNumber);
            //create user
            if (customer == null)
            {
                UserDto newCustomer = new UserDto
                {
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    UserName = new Guid().ToString(),
                    Gender = Gender,
                    RoleId = 3,
                    Name = Name,
                    Password = "cuZXynTq",
                    CreatedDate = DateTime.Now,
                };
                await userService.CreateUser(newCustomer);
                await hubContext.Clients.All.SendAsync("ReloadUsers");
                customer = await userService.GetCustomerByPhoneNumber(PhoneNumber);
            }


            var appointmentDate = DateOnly.FromDateTime(AppointmentTime.Date);
            Dictionary<string, string> result = await appointmentService.CreateAppointmentWithDentist(AppointmentTime, customer.UserId, appointmentDate, SelectedServiceId, SelectedDentistId);
            if (!result.ContainsKey("Success"))
            {
                foreach (var item in result)
                {
                    TempData["Book"] = item.Value;
                }

                Services = await service.GetAllServices();
                return Page();
            }
            TempData["Book"] = "Appointment created successfully!";

            hubContext.Clients.All.SendAsync("ReloadAppointments");
            var receiver = Email;
            var subject = "Thank you for your booking!";
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "ThanksGuestBooking.html");
            string body = await System.IO.File.ReadAllTextAsync(templatePath);
            var selectedService = await service.GetServiceByID(SelectedServiceId);
            body = body.Replace("[Service Details]", selectedService.ServiceName)
                       .Replace("[Date]", DateOnly.FromDateTime(AppointmentTime).ToString())
                       .Replace("[Time]", TimeOnly.FromDateTime(AppointmentTime).ToString())
                       .Replace("[Email]", Email)
                       .Replace("[Password]", "cuZXynTq");

            await emailSender.SendEmailAsync(receiver, subject, body);

            Services = await service.GetAllServices();
            return Page();
        }
    }
}
