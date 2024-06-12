using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DentistBooking.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = ("Missing field Email"))]
        [EmailAddress(ErrorMessage = ("Invalid Email"))]
        public string ForgetEmail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = ("Missing field Password"))]
        [MinLength(8, ErrorMessage = ("Password must has a minimum length of 8"))]
        public string ForgetPassword { get; set; }

        [BindProperty]
        public string ForgetConfirm { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostForget()
        {
            if (ModelState.IsValid)
            {
                // Implement forgot password logic here
                // For example, update the user's password in the database

                // Redirect to a confirmation page or login page after successful password reset
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
