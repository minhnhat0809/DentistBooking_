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

namespace DentistBooking.Pages.AdminPage.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
       

        public IndexModel(IUserService userService)
        {
            _userService = userService; 
        }

        public IPagedList<User> User { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            var users = await  _userService.GetAllUsers();
            User = users.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
