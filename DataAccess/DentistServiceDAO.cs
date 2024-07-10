using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DentistServiceDAO
    {
        private static DentistServiceDAO instance = null;
        private static readonly object instanceLock = new object();
        private DentistServiceDAO() { }
        public static DentistServiceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DentistServiceDAO();
                    }
                    return instance;
                }
            }
        }

        public DentistService getDentistServiceByID(int id)
        {
            var context = new BookingDentistDbContext();
            var dentistService = context.DentistServices.FirstOrDefault(c => c.DentistServiceId == id);
            return dentistService;
        }

        public List<DentistService> getAllDentistServices()
        {
            var context = new BookingDentistDbContext();
            var dentistServices = context.DentistServices.ToList();
            return dentistServices;
        }

        public void deleteUser(DentistService dentistService)
        {
            var context = new BookingDentistDbContext();
            dentistService.Status = false;
            context.Entry<DentistService>(dentistService).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createDentistService(DentistService dentistService)
        {
            var context = new BookingDentistDbContext();
            context.DentistServices.Add(dentistService);
            context.SaveChanges();
        }

        public void updateDentistService(DentistService dentistService)
        {
            var context = new BookingDentistDbContext();
            context.Entry<DentistService>(dentistService).State = EntityState.Modified;
            context.SaveChanges();
        }

        public async Task< List<BusinessObject.Service>> getAllServiceByDentist(int dentistId)
        {
            var context = new BookingDentistDbContext();
            List<BusinessObject.Service> services = await context.DentistServices.Where(ds => ds.DentistId == dentistId).Select(ds => ds.Service).ToListAsync();
            return services;
        }
        
    }
}
