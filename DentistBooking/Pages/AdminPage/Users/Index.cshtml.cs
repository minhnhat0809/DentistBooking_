using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using X.PagedList;
using Service.Impl;
using BusinessObject.DTO;

namespace DentistBooking.Pages.AdminPage.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
       

        public IndexModel(IUserService userService)
        {
            _userService = userService; 
        }

        public IPagedList<UserDto> User { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
            var users = await  _userService.GetAllUsers();
            User = users.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
