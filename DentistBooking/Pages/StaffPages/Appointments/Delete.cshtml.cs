﻿using System;
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
using BusinessObject.Result;
using Microsoft.IdentityModel.Tokens;
using Service.Lib;
using Service.Impl;

namespace DentistBooking.Pages.StaffPages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        public DeleteModel(IAppointmentService appointmentService, IHubContext<SignalRHub> hubContext, IConfiguration configuration, IEmailSender emailSender)
        {
            _appointmentService = appointmentService;   
            _hubContext = hubContext;
            this.configuration = configuration;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (!role.IsNullOrEmpty())
            {
                if (!role.Equals("Staff"))
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/Index");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByID(id.Value);

            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int appointmentId)
        {
            string role = HttpContext.Session.GetString("Role");
            if (!role.IsNullOrEmpty())
            {
                if (!role.Equals("Staff"))
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/Index");
            }

            if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Reason and Customer Name are required.";
                Appointment = await _appointmentService.GetAppointmentByID(appointmentId);
                return Page();
            }

            AppointmentDto oldAppointment = await _appointmentService.GetAppointmentByID(appointmentId);
            AppointmentResult result = await _appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, Reason);
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteAppointment"] = result.Message;
                Appointment =  await _appointmentService.GetAppointmentByID(appointmentId);
                return Page();
            }
            TempData["SuccessDeleteAppointment"] = "Delete successfully!";
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            var receiver = oldAppointment.Customer.Email;
            var subject = "Your appointment has been denied!";
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "SorryForDenied.html");
            string body = await System.IO.File.ReadAllTextAsync(templatePath);
            body = body.Replace("[Service Details]", oldAppointment.Service.ServiceName)
                       .Replace("[Date]", oldAppointment.TimeStart.ToString("yyyy-MM-dd"))
                       .Replace("[Time]", oldAppointment.TimeStart.ToString("HH:mm"))
                       .Replace("[Reason for Denial]", Reason)
                       .Replace("[Contact Information]", "support@gooddentist.com"); 

            await emailSender.SendEmailAsync(receiver, subject, body);

            await emailSender.SendEmailAsync(receiver, subject, body);
            await _hubContext.Clients.All.SendAsync("ReloadAppointments");
            return RedirectToPage("./Index");
        }
    }
}
