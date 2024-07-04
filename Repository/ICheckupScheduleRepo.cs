using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface ICheckupScheduleRepo
    {
        public Task<List<CheckupSchedule>> GetAllCheckupSchedules();
        public Task<CheckupSchedule> GetById(int? id);
        public void CreateCheckupSchedule(CheckupSchedule schedule);
        public void UpdateCheckupSchedule(CheckupSchedule schedule);
        public void DeleteCheckupSchedule(int id);
    }
}
