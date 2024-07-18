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
using BusinessObject.DTO;

namespace DentistBooking.Pages.DentistPage.Customers
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;


        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IPagedList<UserDto> User { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userService.GetAllCustomers();
            users = users.Where(x => x.Status == true).ToList();
            User = users.ToPagedList(PageNumber, PageSize);
            return Page();
        }
    }
}
