using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int? Quantity { get; set; }

        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual MedicineDto? Medicine { get; set; }

        public virtual PrescriptionDto? Prescription { get; set; }
    }
}
