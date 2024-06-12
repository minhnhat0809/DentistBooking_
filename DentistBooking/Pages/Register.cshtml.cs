using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DentistBooking.Pages
{
    public class RegisterModel : PageModel
    {


        [BindProperty]
        [Required]
        [MinLength(5, ErrorMessage = ("Name must has a minimum length of 5 and maximum length 30"))]
        [MaxLength(30, ErrorMessage = ("Name must has a minimum length of 5 and maximum length 30"))]
        public string Name { get; set; }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required]
        public string RegisterEmail { get; set; }

        [BindProperty]
        [Required]
        [MinLength(8, ErrorMessage = ("Password must has a minimum length of 8"))]
        public string RegisterPassword { get; set; }

        [BindProperty]
        public string RegisterConfirm { get; set; }

        [BindProperty]
        [Required]
        public DateTime? Dob { get; set; }

        [BindProperty]
        [Required]

        public string Gender { get; set; }

        [BindProperty]
        [Required]
        [MinLength(10 , ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                // Implement registration logic here
                // For example, save the user data to the database

                // Redirect to a confirmation page or login page after successful registration
                return RedirectToPage("/Users/View");
            }
            else
            {
                // Invalid login attempt
                ModelState.AddModelError(string.Empty, "Email or Password is incorrect");
            }
            return Page();
        }
    }
}
