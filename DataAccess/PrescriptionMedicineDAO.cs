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

        public PrescriptionMedicine getDentistServiceByID(int id)
        {
            var context = new BookingDentistDbContext();
            var prescriptionMedicine = context.PrescriptionMedicines.FirstOrDefault(c => c.PrescriptionMedicineId == id);
            return prescriptionMedicine;
        }

        public List<PrescriptionMedicine> getAllPrescriptionMedicines()
        {
            var context = new BookingDentistDbContext();
            var prescriptionMedicines = context.PrescriptionMedicines.ToList();
            return prescriptionMedicines;
        }

        public void deletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            var context = new BookingDentistDbContext();
            prescriptionMedicine.Status = false;
            context.Entry<PrescriptionMedicine>(prescriptionMedicine).State = EntityState.Modified;
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
