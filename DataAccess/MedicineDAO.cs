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

        public async Task<Medicine> getMedicineByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var medicine = await context.Medicines.FirstOrDefaultAsync(c => c.MedicineId == id);
            return medicine;
        }

        public async Task<List<Medicine>> getAllMedicines()
        {
            var context = new BookingDentistDbContext();
            var medicineList = await context.Medicines.ToListAsync();
            return medicineList;
        }

        public async Task deleteMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            medicine.Status = false;
            context.Entry<Medicine>(medicine).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            context.Medicines.Add(medicine);
            await context.SaveChangesAsync();
        }

        public async Task updateMedicine(Medicine medicine)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Medicine>(medicine).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
