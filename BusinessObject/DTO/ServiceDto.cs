using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = null!;

        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public virtual ICollection<DentistServiceDto> DentistServices { get; set; } = new List<DentistServiceDto>();
    }
}
