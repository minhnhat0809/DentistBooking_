using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DentistBooking.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService userService;

        public IndexModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        public string Email { get; set; } = "";

        [BindProperty]
        [Required]
        public string Password { get; set; } = "";
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogin()
        {
            if (ModelState.IsValid)
            {
                var result = await userService.Login(Email, Password);

                if (result.IsSuccess)
                {
                    HttpContext.Session.SetString("Email", Email);
                    HttpContext.Session.SetString("Role", result.Result.ToString());
                    HttpContext.Session.SetString("ID", result.Message);
                    if (result.Result.Equals("Customer"))
                    {
                        return RedirectToPage("/CustomerPage/ViewServices");
                    }else if (result.Result.Equals("Admin"))
                    {
                        return RedirectToPage("/Users/Index");
                    }else if (result.Result.Equals("Staff"))
                    {
                        return RedirectToPage("/StaffPages/ViewDentistSlot");
                    }
                    else if (result.Result.Equals("Staff"))
                    {
                        return RedirectToPage("/Staff/MedicineManagement/Index");
                    }

                }
                else
                {                   
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
