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
        Task<List<CheckupScheduleDto>> GetAllCheckupSchedules();
        Task CreateCheckupSchedule(CheckupSchedule schedule);
        Task UpdateCheckupSchedule(CheckupSchedule schedule);
        Task DeleteCheckupSchedule(int id);

        Task<CheckupScheduleDto> GetDtoById(int? id);

        Task<CheckupSchedule> GetById(int? id);
    }
}
