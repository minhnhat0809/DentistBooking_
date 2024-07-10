using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class PrescriptionMedicineDto
    {
        public int PrescriptionMedicineId { get; set; }

        public int? PrescriptionId { get; set; }

        public int? MedicineId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual MedicineDto? Medicine { get; set; }

        public virtual PrescriptionDto? Prescription { get; set; }
    }
}
