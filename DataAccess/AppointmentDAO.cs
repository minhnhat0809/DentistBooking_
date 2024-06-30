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
            var appointment = context.Appointments.Include(a => a.DentistSlot.Dentist).Include(a => a.Customer).FirstOrDefault(c => c.AppointmentId == id);
            return appointment;
        }

        public async Task<List<Appointment>> getAllAppointments()
        {
            var context = new BookingDentistDbContext();
            var appointments = await context.Appointments.OrderBy(ap => ap.TimeStart).ToListAsync();
            return appointments;
        }

        
        public async Task<List<Appointment>> getAllAppointmentsOfCustomer(int customerId)
        {
            var context = new BookingDentistDbContext();
            var appointments = await context.Appointments.Include(ap => ap.DentistSlot)
                .ThenInclude(ds => ds.Dentist).Where(ap => ap.CustomerId == customerId).OrderBy(ap => ap.TimeStart).ToListAsync();
            return appointments;
        }

        public void deleteAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            appointment.Status = "Deleted";
            context.Entry<Appointment>(appointment).State = EntityState.Modified;
            context.SaveChanges();
        }

        public async Task createAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();
        }

        public void updateAppointment(Appointment appointment)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Appointment>(appointment).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
