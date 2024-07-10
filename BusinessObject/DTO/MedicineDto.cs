using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class MedicineDto
    {
        public int MedicineId { get; set; }

        public string MedicineName { get; set; } = null!;

        public string? Type { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual ICollection<PrescriptionMedicineDto> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicineDto>();
    }
}
