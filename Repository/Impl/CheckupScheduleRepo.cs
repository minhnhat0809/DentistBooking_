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
        public List<CheckupSchedule> GetAllCheckupSchedules()
            => CheckupScheduleDAO.Instance.getAllCheckupSchedule();

        public CheckupSchedule GetById(int id)
            => CheckupScheduleDAO.Instance.getCheckupScheduleByID(id);

        public void CreateCheckupSchedule(CheckupSchedule schedule)
            => CheckupScheduleDAO.Instance.createCheckupScheducle(schedule);

        public void UpdateCheckupSchedule(CheckupSchedule schedule)
            => CheckupScheduleDAO.Instance.updateCheckupScheducle(schedule);

        public void DeleteCheckupSchedule(int id)
        {
            var model = CheckupScheduleDAO.Instance.getCheckupScheduleByID(id);
            CheckupScheduleDAO.Instance.deleteCheckupSchedule(model);
        } 
    }
}
