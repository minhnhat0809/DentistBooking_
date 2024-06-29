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

        public Service getServiceByID(int id)
        {
            var context = new BookingDentistDbContext();
            var service = context.Services.FirstOrDefault(c => c.ServiceId == id);
            return service;
        }

        public List<Service> getAllServices()
        {
            var context = new BookingDentistDbContext();
            var serviceList = context.Services.ToList();
            return serviceList;
        }

        public void deleteService(Service service)
        {
            var context = new BookingDentistDbContext();
            service.Status = false;
            context.Entry<Service>(service).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createService(Service service)
        {
            var context = new BookingDentistDbContext();
            context.Services.Add(service);
            context.SaveChanges();
        }

        public void updateService(Service service)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Service>(service).State = EntityState.Modified;
            context.SaveChanges();
        }

    }
}
