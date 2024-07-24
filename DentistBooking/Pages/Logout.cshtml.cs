using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentistBooking.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            return RedirectToPage("/Index");
        }
    }
}
