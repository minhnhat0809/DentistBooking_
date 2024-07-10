using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using X.PagedList;

namespace DentistBooking.Pages.StaffPages.Users;

public class ViewUsers : PageModel
{
    private readonly IUserService userService;

    public ViewUsers(IUserService userService)
    {
        this.userService = userService;
    }

    public IList<UserDto> Users { get; set; } = default!;


    [BindProperty(SupportsGet = true)]
    public string SelectedUserType { get; set; } = default!;

    public List<string> Types = new List<string> { "All", "Dentist", "Customer" };

    public void OnGet()
    {
        Users = userService.GetAllUserByType(SelectedUserType).Result;

    }
}