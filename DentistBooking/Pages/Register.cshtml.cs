using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Service;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public RegisterModel(IUserService userService, IHubContext<SignalRHub> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters long.")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string RegisterEmail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string RegisterPassword { get; set; }

        [BindProperty]
        [Compare("RegisterPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string RegisterConfirm { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime Dob { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                if (!RegisterPassword.Equals(RegisterConfirm))
                {
                    TempData["ErrorMessage"] = "Confirm password is not the same.";
                    return Page();
                }

                var dupliUser = await _userService.GetCustomerByPhoneNumber(PhoneNumber);
                if (dupliUser != null)
                {
                    TempData["ErrorMessage"] = "This phone number has been registered!";
                    return Page();
                }

                dupliUser = await _userService.GetCustomerByEmail(RegisterEmail);
                if (dupliUser != null)
                {
                    TempData["ErrorMessage"] = "This email address has been registered!";
                    return Page();
                }

                UserDto newUser = new UserDto()
                {
                    Name = Name,
                    UserName = RegisterEmail,
                    Email = RegisterEmail,
                    Password = RegisterPassword,
                    PhoneNumber = PhoneNumber,
                    Gender = Gender,
                    Dob = DateOnly.FromDateTime(Dob),
                    RoleId = 3,
                    CreatedDate = DateTime.Now,
                    Status = true
                };
                await _userService.CreateUser(newUser);
                await _hubContext.Clients.All.SendAsync("ReloadUsers");
                HttpContext.Session.SetString("Email", RegisterEmail);
                HttpContext.Session.SetString("Role", "3");
                HttpContext.Session.SetString("ID", newUser.UserId.ToString());
                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToPage("/CustomerPage/ViewAppointments");
            }

            TempData["ErrorMessage"] = "There are some errors in the form. Please correct them and try again.";
            return Page();
        }
    }
}
