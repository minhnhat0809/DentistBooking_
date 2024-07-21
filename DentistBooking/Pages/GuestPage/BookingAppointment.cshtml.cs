using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Impl;
using Service.Lib;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.Pages.GuestPage
{
    public class BookingAppointmentModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IService service;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public BookingAppointmentModel(IUserService userService, IAppointmentService appointmentService, IService service, IEmailSender emailSender)
        {
            this.appointmentService = appointmentService;
            this.service = service;
            this.userService = userService;
            this.emailSender = emailSender;
        }

        public List<ServiceDto> Services { get; set; } = new List<ServiceDto>();

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
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Dob { get; set; }

        public async Task OnGetAsync()
        {
            Services = await service.GetAllServices();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Services = await service.GetAllServices();
                return Page();
            }

            var customer = await userService.GetCustomerByPhoneNumber(PhoneNumber);

            // Create user if not exist
            if (customer == null)
            {
                UserDto newCustomer = new UserDto
                {
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    UserName = Guid.NewGuid().ToString(),
                    Gender = Gender,
                    RoleId = 3,
                    Dob = Dob,
                    Name = Name,
                    Password = "cuZXynTq"
                };
                await userService.CreateUser(newCustomer);
                customer = await userService.GetCustomerByPhoneNumber(PhoneNumber);
            }

            var appointmentDate = DateOnly.FromDateTime(AppointmentTime.Date);

            Dictionary<string, string> result = await appointmentService.CreateAppointment(AppointmentTime, customer.UserId, appointmentDate, SelectedServiceId);
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
            var receiver = Email;
            var subject = "Thank you for your booking!";
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ThanksGuestBooking.html");
            string body = await System.IO.File.ReadAllTextAsync(templatePath);
            var selectedService = await service.GetServiceByID(SelectedServiceId);
            body = body.Replace("[Service Details]", selectedService.ServiceName)
                       .Replace("[Date]", DateOnly.FromDateTime(AppointmentTime).ToString())
                       .Replace("[Time]", TimeOnly.FromDateTime(AppointmentTime).ToString());

            await emailSender.SendEmailAsync(receiver, subject, body);

            Services = await service.GetAllServices();
            return Page();
        }
    }
}
