using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ClinicDAO
    {
        private static ClinicDAO instance = null;
        private static readonly object instanceLock = new object();
        private ClinicDAO() { }
        public static ClinicDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ClinicDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Clinic> getClinicByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var clinic = await context.Clinics.FirstOrDefaultAsync(c => c.ClinicId == id);
            return clinic;
        }

        public async Task<List<Clinic>> getAllClinics()
        {
            var context = new BookingDentistDbContext();
            var clinicList = await context.Clinics.ToListAsync();
            return clinicList;
        }

        public void deleteClinic(Clinic clinic)
        {
            var context = new BookingDentistDbContext();
            clinic.Status = false;
            context.Entry<Clinic>(clinic).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createClinic(Clinic clinic)
        {
            var context = new BookingDentistDbContext();
            context.Clinics.Add(clinic);
            context.SaveChanges();
        }

        public void updateClinic(Clinic clinic)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Clinic>(clinic).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
