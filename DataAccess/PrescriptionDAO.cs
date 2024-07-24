using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PrescriptionDAO
    {
        private static PrescriptionDAO instance = null;
        private static readonly object instanceLock = new object();
        private PrescriptionDAO() { }
        public static PrescriptionDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PrescriptionDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Prescription> getPrescriptionByID(int id)
        {
            var context = new BookingDentistDbContext();
            var prescription = await context.Prescriptions
                .Include(x=>x.Appointment) 
                .Include(x=>x.PrescriptionMedicines)
                .FirstOrDefaultAsync(c => c.PrescriptionId == id);
            return prescription;
        }

        public async Task<List<Prescription>> getAllPrescriptions()
        {
            var context = new BookingDentistDbContext();
            var prescriptionList = await context.Prescriptions
                .Include (x=>x.Appointment)
                .ThenInclude(x=>x.Customer)
                .ToListAsync();
            return prescriptionList;
        }
        public async Task<Prescription> GetByIdWithMedicinesAsync(int id)
        {
            var context = new BookingDentistDbContext();
            var prescriptionList = await context.Prescriptions
                             .Include(x=>x.Appointment)
                             .Include(p => p.PrescriptionMedicines)
                             .ThenInclude(pm => pm.Medicine)
                             .FirstOrDefaultAsync(p => p.PrescriptionId == id);
            return prescriptionList;
        }

        public async Task deletePrescription(Prescription prescription)
        {
            var context = new BookingDentistDbContext();
            prescription.Status = false;
            context.Entry<Prescription>(prescription).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createPrescription(Prescription prescription)
        {
            var context = new BookingDentistDbContext();
            context.Prescriptions.Add(prescription);
            await context.SaveChangesAsync();
        }

        public async Task updatePrescription(Prescription prescription)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Prescription>(prescription).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
