﻿using System;
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

        public async Task CreateCheckupSchedule(CheckupSchedule schedule)
            => await CheckupScheduleDAO.Instance.createCheckupScheducle(schedule);

        public async Task UpdateCheckupSchedule(CheckupSchedule schedule)
            => await CheckupScheduleDAO.Instance.updateCheckupScheducle(schedule);

        public async Task DeleteCheckupSchedule(int id)
        {
            var model = await CheckupScheduleDAO.Instance.getCheckupScheduleByID(id);
            await CheckupScheduleDAO.Instance.deleteCheckupSchedule(model);
        } 
    }
}
