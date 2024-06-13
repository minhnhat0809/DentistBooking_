using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DentistBooking.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPostLogin()
        {
            if (ModelState.IsValid)
            {
                // Replace this with your actual login logic

                if (Email == "admin@domain.com" && Password == "password")
                {
                    // On successful login, redirect to a different page, e.g., Dashboard
                    return RedirectToPage("/Users/View");
                }
                else
                {
                    // Invalid login attempt
                    ModelState.AddModelError(string.Empty, "Email or Password is incorrect");
                }
            }
            return Page();
        }


        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
