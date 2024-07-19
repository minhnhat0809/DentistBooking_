using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DentistSlotDAO
    {
        private static DentistSlotDAO instance = null;
        private static readonly object instanceLock = new object();
        private DentistSlotDAO() { }
        public static DentistSlotDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DentistSlotDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<DentistSlot> getDentistSlotByID(int id)
        {
            var context = new BookingDentistDbContext();
            var dentistSlot = await context.DentistSlots
                .Include(x=> x.Dentist)
                .Include(ds => ds.Appointments)
                .FirstOrDefaultAsync(c => c.DentistSlotId == id);
            return dentistSlot;
        }

        public async Task<List<DentistSlot>> getAllDentistSlots()
        {
            var context = new BookingDentistDbContext();
            var dentistSlotList = await context.DentistSlots
                .Include(x=>x.Dentist)
                .ToListAsync();
            return dentistSlotList;
        }       
        public async Task<List<DentistSlot>> getAllDentistSlotsByDentist(int id, DateOnly selectedDate)
        {
            var context = new BookingDentistDbContext();
            var dentistSlotList = await context.DentistSlots.Include(ds => ds.Appointments)
                .Where(ds => ds.DentistId == id && DateOnly.FromDateTime(ds.TimeStart.Date).Equals(selectedDate))
                .OrderBy(ds => ds.TimeStart).ToListAsync();
            return dentistSlotList;
        }

        public async Task deleteDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            dentistSlot.Status = false;
            context.Entry<DentistSlot>(dentistSlot).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            context.DentistSlots.Add(dentistSlot);
            await context.SaveChangesAsync();
        }

        public async Task updateDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            context.Entry<DentistSlot>(dentistSlot).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
