using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ServiceDAO
    {
        private static ServiceDAO instance = null;
        private static readonly object instanceLock = new object();
        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Service> getServiceByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var service = await context.Services.FirstOrDefaultAsync(c => c.ServiceId == id);
            return service;
        }

        public async Task<List<Service>> getAllServices()
        {
            var context = new BookingDentistDbContext();
            var serviceList = await context.Services.ToListAsync();
            return serviceList;
        }
        public async Task<List<Service>> GetServicesByDentistSlotAsync(int dentistSlotId)
        {
            var context = new BookingDentistDbContext();
            var services = await context.DentistSlots
                .Where(ds => ds.DentistSlotId == dentistSlotId)
                .SelectMany(ds => ds.Dentist.DentistServices)
                .Where(ds => ds.Status == true)
                .Select(ds => ds.Service)
                .Distinct()
                .ToListAsync();
            return services;
        }

        public async Task deleteService(Service service)
        {
            var context = new BookingDentistDbContext();
            service.Status = false;
            context.Entry<Service>(service).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createService(Service service)
        {
            var context = new BookingDentistDbContext();
            context.Services.Add(service);
            await context.SaveChangesAsync();
        }

        public async Task updateService(Service service)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Service>(service).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

    }
}
