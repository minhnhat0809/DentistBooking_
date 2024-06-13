using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MedicineDAO
    {
        private static MedicineDAO instance = null;
        private static readonly object instanceLock = new object();
        private MedicineDAO() { }
        public static MedicineDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MedicineDAO();
                    }
                    return instance;
                }
            }
        }

        public Medicine getMedicineByID(int id)
        {
            var context = new BookingDentistDbContext();
            var medicine = context.Medicines.FirstOrDefault(c => c.MedicineId == id);
            return medicine;
        }

        public List<Medicine> getAllMedicines()
        {
            var context = new BookingDentistDbContext();
            var medicineList = context.Medicines.ToList();
            return medicineList;
        }

        public void deleteMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            medicine.Status = false;
            context.Entry<Medicine>(medicine).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            context.Medicines.Add(medicine);
            context.SaveChanges();
        }

        public void updateMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Medicine>(medicine).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
