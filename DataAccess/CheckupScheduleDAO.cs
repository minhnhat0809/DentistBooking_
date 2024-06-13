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

        public CheckupSchedule getCheckupScheduleByID(int id)
        {
            var context = new BookingDentistDbContext();
            var clinic = context.CheckupSchedules.FirstOrDefault(c => c.ScheduleId == id);
            return clinic;
        }

        public List<CheckupSchedule> getAllCheckupSchedulesByCustomerID(int id)
        {
            var context = new BookingDentistDbContext();
            var checkupScheducleList = context.CheckupSchedules.Where(c => c.CustomerId == id).ToList();
            return checkupScheducleList;
        }

        public void deleteCheckupSchedule(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            checkupScheducle.Status = false;
            context.Entry<CheckupSchedule>(checkupScheducle).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createCheckupScheducle(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            context.CheckupSchedules.Add(checkupScheducle);
            context.SaveChanges();
        }

        public void updateCheckupScheducle(CheckupSchedule checkupScheducle)
        {
            var context = new BookingDentistDbContext();
            context.Entry<CheckupSchedule>(checkupScheducle).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
