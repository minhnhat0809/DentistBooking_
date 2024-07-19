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
        Task<List<CheckupSchedule>> GetAllCheckupSchedules();
        Task<CheckupSchedule> GetById(int? id);
        Task CreateCheckupSchedule(CheckupSchedule schedule);
        Task UpdateCheckupSchedule(CheckupSchedule schedule);
        Task DeleteCheckupSchedule(int id);
    }
}
