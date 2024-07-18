using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ServiceDto
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Service name must be between 1 and 100 characters.")]
        public string ServiceName { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public virtual ICollection<DentistServiceDto> DentistServices { get; set; } = new List<DentistServiceDto>();
    }
}
