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

        public DentistSlot getDentistSlotByID(int id)
        {
            var context = new BookingDentistDbContext();
            var dentistSlot = context.DentistSlots.FirstOrDefault(c => c.DentistSlotId == id);
            return dentistSlot;
        }

        public List<DentistSlot> getAllClinics()
        {
            var context = new BookingDentistDbContext();
            var dentistSlotList = context.DentistSlots.ToList();
            return dentistSlotList;
        }

        public void deleteDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            dentistSlot.Status = false;
            context.Entry<DentistSlot>(dentistSlot).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            context.DentistSlots.Add(dentistSlot);
            context.SaveChanges();
        }

        public void updateDentistSlot(DentistSlot dentistSlot)
        {
            var context = new BookingDentistDbContext();
            context.Entry<DentistSlot>(dentistSlot).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
