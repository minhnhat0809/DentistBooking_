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
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMedicalRecordService _medicalRecordService;

        public DetailsModel(IUserService userService, IMedicalRecordService medicalRecordService)
        {
            _userService = userService;
            _medicalRecordService = medicalRecordService;
        }

        public UserDto User { get; set; } = default!;
        public IPagedList<MedicalRecordDto> MedicalRecords { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
                var medicals = await _medicalRecordService.GetMedicalRecordsByCustomerIdAsync(user.UserId);
                MedicalRecords = medicals.ToPagedList(PageNumber, PageSize);
            }
            return Page();
        }
    }
}
