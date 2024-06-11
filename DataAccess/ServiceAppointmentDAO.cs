using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ServiceAppointmentDAO
    {
        private static ServiceAppointmentDAO instance = null;
        private static readonly object instanceLock = new object();
        private ServiceAppointmentDAO() { }
        public static ServiceAppointmentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceAppointmentDAO();
                    }
                    return instance;
                }
            }
        }

        public ServiceAppointment getDentistServiceByID(int id)
        {
            var context = new BookingDentistDbContext();
            var serviceAppointment = context.ServiceAppointments.FirstOrDefault(c => c.ServiceAppointmentId == id);
            return serviceAppointment;
        }

        public List<ServiceAppointment> getAllServiceAppointments()
        {
            var context = new BookingDentistDbContext();
            var serviceAppointments = context.ServiceAppointments.ToList();
            return serviceAppointments;
        }

        public void deleteServiceAppointment(ServiceAppointment serviceAppointment)
        {
            var context = new BookingDentistDbContext();
            serviceAppointment.Status = false;
            context.Entry<ServiceAppointment>(serviceAppointment).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createServiceAppointment(ServiceAppointment serviceAppointment)
        {
            var context = new BookingDentistDbContext();
            context.ServiceAppointments.Add(serviceAppointment);
            context.SaveChanges();
        }

        public void updateServiceAppointment(ServiceAppointment serviceAppointment)
        {
            var context = new BookingDentistDbContext();
            context.Entry<ServiceAppointment>(serviceAppointment).State = EntityState.Modified;
            context.SaveChanges();
        }        
    }
}
