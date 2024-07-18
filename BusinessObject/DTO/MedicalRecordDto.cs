using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class MedicalRecordDto
    {
        public int MediaRecordId { get; set; }

        public DateTime TimeStart { get; set; }

        public TimeOnly Duration { get; set; }
        [Required(ErrorMessage = "Medical Record Diagnosis is required.")]
        [StringLength(100, ErrorMessage = "Diagnosis can't be longer than 100 characters.")]
        public string? Diagnosis { get; set; }

        public bool? Status { get; set; }

        public int? CustomerId { get; set; }

        public virtual ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public virtual UserDto? Customer { get; set; }
    }
}
