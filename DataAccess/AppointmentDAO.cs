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

        public async Task<Appointment> getAppointmnentByID(int id)
        {
            var context = new BookingDentistDbContext();
            var appointment = await context.Appointments
                .Include(x => x.Customer)
                .Include(x => x.DentistSlot)
                .ThenInclude(d => d.Dentist)
                .Include(x => x.MedicalRecord).ThenInclude(m => m.Customer)
                .Include(x => x.Service)
                .Include(x => x.CreateByNavigation)
                .Include(x => x.ModifiedByNavigation)
                    .FirstOrDefaultAsync(c => c.AppointmentId == id);
                
            return appointment;
        }

        public async Task<List<Appointment>> getAllAppointments()
        {
            var context = new BookingDentistDbContext();
            var appointments = await context.Appointments
                .Include(x=>x.Customer)
                .Include(x => x.DentistSlot).ThenInclude(dl => dl.Dentist)
                .Include(x => x.MedicalRecord)
                .Include(s => s.Service)
                .OrderBy(ap => ap.TimeStart).ToListAsync();
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
            appointment.Status = "Delete";
            context.Appointments.Update(appointment);
            //context.Entry<Appointment>(appointment).State = EntityState.Modified;
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

        public async Task<List<Appointment>> getAllProcessingAppointment()
        {
            var context = new BookingDentistDbContext();
            var appointments = await  context.Appointments
                .Include(ap => ap.DentistSlot)
                .ThenInclude(dl => dl.Dentist)
                .Include(c => c.Customer) 
                .Include(c => c.CreateByNavigation)
                .Include(x=>x.MedicalRecord)
                .Include(m => m.ModifiedByNavigation)
                .Include(s => s.Service)
                .Where(ap => ap.Status.Equals("Processing")).ToListAsync();
            return appointments;
        }
    }
}
