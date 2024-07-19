using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class DentistSlotDto
    {
        public int DentistSlotId { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }

        public bool? Status { get; set; }

        public int? DentistId { get; set; }

        public virtual ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public virtual UserDto? Dentist { get; set; }
    }
}
