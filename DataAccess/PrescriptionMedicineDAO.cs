using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PrescriptionMedicineDAO
    {
        private static PrescriptionMedicineDAO instance = null;
        private static readonly object instanceLock = new object();
        private PrescriptionMedicineDAO() { }
        public static PrescriptionMedicineDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PrescriptionMedicineDAO();
                    }
                    return instance;
                }
            }
        }
        
        public async Task<PrescriptionMedicine> GetByID(int id)
        {
            var context = new BookingDentistDbContext();
            var prescriptionMedicine = await context.PrescriptionMedicines.Include(x => x.Prescription)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(c => c.PrescriptionMedicineId == id);
            return prescriptionMedicine;
        }
        public async Task< List<PrescriptionMedicine>> getAllPrescriptionMedicines()
        {
            var context = new BookingDentistDbContext();
            var prescriptionMedicines = await context.PrescriptionMedicines
                .Include(x => x.Prescription)
                .ThenInclude(p => p.Appointment)
                .Include(x=>x.Medicine)
                .ToListAsync();
            return prescriptionMedicines;
        }
        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int id)
        {
            var context = new BookingDentistDbContext();
            var prescriptionMedicines = await context.PrescriptionMedicines
                .Include(x => x.Prescription)
                .Include(x=>x.Medicine)
                .Where(pm => pm.PrescriptionId == id)
                .ToListAsync();
            return prescriptionMedicines;
        }

        public void deletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            var context = new BookingDentistDbContext();
            context.PrescriptionMedicines.Remove(prescriptionMedicine);
            context.SaveChanges();
        }

        public void createPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            var context = new BookingDentistDbContext();
            context.PrescriptionMedicines.Add(prescriptionMedicine);
            context.SaveChanges();
        }

        public void updatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            var context = new BookingDentistDbContext();
            context.Entry<PrescriptionMedicine>(prescriptionMedicine).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
