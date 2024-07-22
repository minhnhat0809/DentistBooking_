using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Service;
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

    public void OnGet()
    {
        Users = userService.GetAllUserByType(SelectedUserType).Result;
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            Users = userService.GetAllUserByType(SelectedUserType).Result;
            return Page();
        }

        User.CreatedDate = DateTime.Now;
            
        User.Status = true;
        User.RoleId = 3;
        await userService.CreateUser(User);
        await _hubContext.Clients.All.SendAsync("ReloadUsers");
        
        Users = userService.GetAllUserByType(SelectedUserType).Result;
        return Page();
    }
}