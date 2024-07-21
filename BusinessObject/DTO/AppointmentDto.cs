using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }

        public string? Diagnosis { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public int? DentistSlotId { get; set; }

        public int? CustomerId { get; set; }

        public int? ServiceId { get; set; }

        public int? MedicalRecordId { get; set; }

        public int? CreateBy { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual UserDto? CreateByNavigation { get; set; }

        public virtual UserDto? Customer { get; set; }

        public virtual DentistSlotDto? DentistSlot { get; set; }

        public virtual MedicalRecordDto? MedicalRecord { get; set; }

        public virtual UserDto? ModifiedByNavigation { get; set; }

        public virtual ICollection<PrescriptionDto> Prescriptions { get; set; } = new List<PrescriptionDto>();

        public virtual ServiceDto? Service { get; set; }
    }
}
