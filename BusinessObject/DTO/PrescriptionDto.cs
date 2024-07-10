using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class PrescriptionDto
    {
        public int PrescriptionId { get; set; }

        public DateOnly Date { get; set; }

        public string? Diagnosis { get; set; }

        public decimal? Total { get; set; }

        public bool? Status { get; set; }

        public int? AppointmentId { get; set; }

        public virtual AppointmentDto? Appointment { get; set; }

        public virtual ICollection<PrescriptionMedicineDto> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicineDto>();
    }
}
