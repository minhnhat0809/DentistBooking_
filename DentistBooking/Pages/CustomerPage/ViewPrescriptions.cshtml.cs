using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service;
using Service.Impl;
using System.Drawing.Printing;
using X.PagedList;

namespace DentistBooking.Pages.CustomerPage;

public class ViewPrescriptions : PageModel
{
    private readonly IPrescriptionService prescriptionService;
    private readonly IUserService userService;

    public ViewPrescriptions(IPrescriptionService prescriptionService, IUserService userService)
    {
        this.prescriptionService = prescriptionService;
        this.userService = userService;
    }
    public IPagedList<PrescriptionDto> Prescriptions { get; set; }

    [BindProperty(SupportsGet = true)]

    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]

    public int PageSize { get; set; } = 5;

    public async Task<IActionResult> OnGet()
    {
        string? email = HttpContext.Session.GetString("Email");
        if (!string.IsNullOrEmpty(email))
        {
            var users = await userService.GetAllDentists();
            if (users != null)
            {
                var user = users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "No dentist found with the provided email.";
                    return RedirectToPage("/Error");
                }
                var prescriptions = await prescriptionService.GetAllPrescriptionByCustomer(user.UserId);
                Prescriptions = prescriptions.ToPagedList(PageNumber, PageSize);
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = "No dentist found with the provided email.";
                return RedirectToPage("/Error");
            }
        }
        else return RedirectToPage("/Denied");
    }
}