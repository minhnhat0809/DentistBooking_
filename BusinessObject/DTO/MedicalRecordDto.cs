using System;
using System.Collections.Generic;
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

        public string? Diagnosis { get; set; }

        public bool? Status { get; set; }

        public string? CustomerName { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual User? Customer { get; set; }
    }
}
