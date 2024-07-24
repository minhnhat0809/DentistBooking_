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
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.AdminPage.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;
        public DeleteModel(IUserService userService, IHubContext<SignalRHub> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public UserDto User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToPage("/Denied");
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id.Value);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id.Value);
            if (user != null)
            {
                User = user;
                await _userService.DeleteUser(user);
                await _hubContext.Clients.All.SendAsync("ReloadUsers");
            }

            return RedirectToPage("./Index");
        }
    }
}
