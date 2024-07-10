using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class DentistServiceDto
    {
        public int DentistServiceId { get; set; }

        public int? ServiceId { get; set; }

        public int? DentistId { get; set; }

        public bool? Status { get; set; }

        public virtual UserDto? Dentist { get; set; }

        public virtual ServiceDto? Service { get; set; }
    }
}
