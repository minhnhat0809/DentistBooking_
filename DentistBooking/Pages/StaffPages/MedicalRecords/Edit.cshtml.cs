﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Service;
using Microsoft.AspNetCore.SignalR;
using BusinessObject.DTO;

namespace DentistBooking.Pages.StaffPages.MedicalRecords
{
    public class EditModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IMedicalRecordService medicalRecordService, IUserService userService, IHubContext<SignalRHub> hubContext)
        { 
            _medicalRecordService = medicalRecordService;   
            _userService = userService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public MedicalRecordDto MedicalRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Staff")
            {
                return RedirectToPage("/Denied");
            }
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _medicalRecordService.GetById(id);
            if (medicalrecord == null)
            {
                return NotFound();
            }
            MedicalRecord = medicalrecord;
            ViewData["CustomerId"] = new SelectList(_userService.GetAllUsers().Result, "UserId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CustomerId"] = new SelectList(await _userService.GetAllUsers(), "UserId", "Name");
                return Page();
            }

            try
            {
                MedicalRecord.TimeStart = DateTime.Now;
                MedicalRecord.Duration = TimeOnly.FromDateTime(MedicalRecord.TimeStart);
                await _medicalRecordService.UpdateMedicalRecord(MedicalRecord);
                await _hubContext.Clients.All.SendAsync("ReloadMedicalRecords");

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MedicalRecordExists(MedicalRecord.MediaRecordId))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Concurrency error: The record was modified by another user.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the medical record: {ex.Message}");
                return Page();
            }
        }

        private async Task<bool> MedicalRecordExists(int id)
        {
            var records = await _medicalRecordService.GetAllMedicalRecords();
            return records.Any(e => e.MediaRecordId == id);
        }
    }
}
