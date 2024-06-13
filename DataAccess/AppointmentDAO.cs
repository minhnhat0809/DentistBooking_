using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppointmentDAO
    {
        private static AppointmentDAO instance = null;
        private static readonly object instanceLock = new object();
        private AppointmentDAO() { }
        public static AppointmentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AppointmentDAO();
                    }
                    return instance;
                }
            }
        }

        public Appointment getAppointmnentByID(int id)
        {
            var context = new BookingDentistDbContext();
            var category = context.Appointments.FirstOrDefault(c => c.AppointmentId == id);
            return category;
        }

        public List<Appointment> getAllAppointments()
        {
            var context = new BookingDentistDbContext();
            var categoryList = context.Appointments.ToList();
            return categoryList;
        }

        public void deleteAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            appointment.Status = false;
            context.Entry<Appointment>(appointment).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            context.Appointments.Add(appointment);
            context.SaveChanges();
        }

        public void updateAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Appointment>(appointment).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
