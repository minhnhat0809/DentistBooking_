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

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual ICollection<DentistService> DentistServices { get; set; } = new List<DentistService>();
    }
}
