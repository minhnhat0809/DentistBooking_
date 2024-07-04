using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class CheckupScheduleRepo : ICheckupScheduleRepo
    {
        public async Task<List<CheckupSchedule>> GetAllCheckupSchedules()
            => await CheckupScheduleDAO.Instance.getAllCheckupSchedule();

        public async Task<CheckupSchedule> GetById(int? id)
            => await CheckupScheduleDAO.Instance.getCheckupScheduleByID(id);

        public void CreateCheckupSchedule(CheckupSchedule schedule)
            => CheckupScheduleDAO.Instance.createCheckupScheducle(schedule);

        public void UpdateCheckupSchedule(CheckupSchedule schedule)
            => CheckupScheduleDAO.Instance.updateCheckupScheducle(schedule);

        public async void DeleteCheckupSchedule(int id)
        {
            var model = await CheckupScheduleDAO.Instance.getCheckupScheduleByID(id);
            CheckupScheduleDAO.Instance.deleteCheckupSchedule(model);
        } 
    }
}
