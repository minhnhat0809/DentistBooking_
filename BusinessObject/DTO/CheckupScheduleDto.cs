using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CheckupScheduleDto
    {
        public int ScheduleId { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }

        public string? Notes { get; set; }

        public bool? Status { get; set; }

        public string? CustomerName { get; set; }

        public string? DentistName { get; set; }

        public virtual User? Customer { get; set; }

        public virtual User? Dentist { get; set; }
    }
}
