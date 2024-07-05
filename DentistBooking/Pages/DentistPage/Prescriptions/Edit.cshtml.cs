using System;
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

namespace DentistBooking.Pages.DentistPage.Prescriptions
{
    public class EditModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMedicineService _medicineService; // New service to handle medicines
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(IPrescriptionService prescriptionService,
            IAppointmentService appointmentService, IMedicineService medicineService,
            IHubContext<SignalRHub> hubContext)
        {
            _prescriptionService = prescriptionService;
            _appointmentService = appointmentService;
            _medicineService = medicineService; // Initialize the new service
            _hubContext = hubContext;
        }

        [BindProperty]
        public Prescription Prescription { get; set; } = default!;

        [BindProperty]
        public List<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _prescriptionService.GetByIdWithMedicinesAsync(id.Value);
            if (prescription == null)
            {
                return NotFound();
            }

            Prescription = prescription;
            PrescriptionMedicines = Prescription.PrescriptionMedicines.ToList();

            ViewData["AppointmentId"] = new SelectList( _appointmentService.GetAllAppointments().Result , "AppointmentId", "AppointmentId");
            ViewData["MedicineId"] = new SelectList( _medicineService.GetAllMedicines(), "MedicineId", "Name");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["AppointmentId"] = new SelectList(await _appointmentService.GetAllAppointments(), "AppointmentId", "AppointmentId");
                ViewData["MedicineId"] = new SelectList( _medicineService.GetAllMedicines(), "MedicineId", "Name");
                return Page();
            }

            try
            {
                _prescriptionService.UpdatePrescription(Prescription);

                foreach (var prescriptionMedicine in PrescriptionMedicines)
                {
                    if (prescriptionMedicine.PrescriptionMedicineId == 0)
                    {
                        // Add new PrescriptionMedicine
                        _prescriptionService.AddPrescriptionMedicine(prescriptionMedicine);
                    }
                    else
                    {
                        // Update existing PrescriptionMedicine
                        _prescriptionService.UpdatePrescriptionMedicine(prescriptionMedicine);
                    }
                }

                await _hubContext.Clients.All.SendAsync("ReloadPrescriptions");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionExists(Prescription.PrescriptionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PrescriptionExists(int id)
        {
            return _prescriptionService.GetPrescriptions().Any(e => e.PrescriptionId == id);
        }
    }
}
