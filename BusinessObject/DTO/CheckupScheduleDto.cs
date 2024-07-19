using BusinessObject.ValidationDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CheckupScheduleDto
    {
        public CheckupScheduleDto()
        {
            TimeStart = DateTime.Now;
        }
        public int ScheduleId { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }
        [Required]
        public string? Notes { get; set; }

        public bool? Status { get; set; }

        public string? CustomerName { get; set; }

        public string? DentistName { get; set; }

        public virtual User? Customer { get; set; }

        public virtual User? Dentist { get; set; }
    }
}
