using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{
    public interface ICheckupScheduleService
    {
        public List<CheckupSchedule> GetAllCheckupSchedules();
        public CheckupSchedule GetById(int id);
        public void CreateCheckupSchedule(CheckupSchedule schedule);
        public void UpdateCheckupSchedule(CheckupSchedule schedule);
        public void DeleteCheckupSchedule(int id);
    }
}
