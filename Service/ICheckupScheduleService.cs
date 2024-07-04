using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface ICheckupScheduleService
    {
        public Task<List<CheckupScheduleDto>> GetAllCheckupSchedules();
        public void CreateCheckupSchedule(CheckupSchedule schedule);
        public void UpdateCheckupSchedule(CheckupSchedule schedule);
        public void DeleteCheckupSchedule(int id);

        public Task<CheckupScheduleDto> GetDtoById(int? id);

        public Task<CheckupSchedule> GetById(int? id);
    }
}
