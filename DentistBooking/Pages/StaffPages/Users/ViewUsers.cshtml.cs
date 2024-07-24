using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Impl;
using X.PagedList;

namespace DentistBooking.Pages.StaffPages.Users;

public class ViewUsers : PageModel
{
    private readonly IUserService userService;
    private readonly IHubContext<SignalRHub> _hubContext;

    public ViewUsers(IUserService userService, IHubContext<SignalRHub> hubContext)
    {
        this.userService = userService;
        _hubContext = hubContext;
    }

    
    public IList<UserDto> Users { get; set; } = default!;

    [BindProperty]
    public UserDto User { get; set; } = default!;


    [BindProperty(SupportsGet = true)]
    public string SelectedUserType { get; set; } = default!;

    public List<string> Types = new List<string> { "All", "Dentist", "Customer" };

    public async Task<IActionResult> OnGet()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff")
        {
            return RedirectToPage("/Denied");
        }
        Users = await userService.GetAllUserByType(SelectedUserType);
        return Page();
    }

    
    
}