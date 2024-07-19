using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CheckupScheduleDAO
    {
        private static CheckupScheduleDAO instance = null;
        private static readonly object instanceLock = new object();
        private CheckupScheduleDAO() { }
        public static CheckupScheduleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CheckupScheduleDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<CheckupSchedule>> getAllCheckupSchedule()
        {
            var context = new BookingDentistDbContext();
            var scheduleList = await context.CheckupSchedules
                .Include(x=>x.Customer)
                .ToListAsync();
            return scheduleList;
        }
        public async Task<CheckupSchedule> getCheckupScheduleByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var clinic = await context.CheckupSchedules
                .Include(x=>x.Customer)
                .FirstOrDefaultAsync(c => c.ScheduleId == id);
            return clinic;
        }

        public List<CheckupSchedule> getAllCheckupSchedulesByCustomerID(int id)
        {
            var context = new BookingDentistDbContext();
            var checkupScheducleList = context.CheckupSchedules.Where(c => c.CustomerId == id).ToList();
            return checkupScheducleList;
        }

        public async Task deleteCheckupSchedule(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            checkupScheducle.Status = false;
            context.Entry<CheckupSchedule>(checkupScheducle).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createCheckupScheducle(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            context.CheckupSchedules.Add(checkupScheducle);
            await context.SaveChangesAsync();
        }

        public async Task updateCheckupScheducle(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            context.Entry<CheckupSchedule>(checkupScheducle).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
