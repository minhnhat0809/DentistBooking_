using BusinessObject.ValidationDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class PrescriptionDto
    {
        public int PrescriptionId { get; set; }

        public DateOnly Date { get; set; }

        [Required]
        public string? Diagnosis { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Total must be a positive value.")]
        public decimal? Total { get; set; }

        public bool? Status { get; set; }

        [Required]
        public int? AppointmentId { get; set; }

        public virtual AppointmentDto? Appointment { get; set; }

        public virtual ICollection<PrescriptionMedicineDto> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicineDto>();
    }
}
