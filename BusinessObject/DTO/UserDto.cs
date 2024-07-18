using BusinessObject.ValidationDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Username must be between 1 and 100 characters.")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = null!;

        [Required]
        [DateRange("1950-01-01", "now", ErrorMessage = "The date must be in an acceptable range.")]
        public DateOnly? Dob { get; set; }

        [StringLength(10, ErrorMessage = "Gender must be up to 10 characters long.")]
        public string? Gender { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? Status { get; set; }

        public int? ClinicId { get; set; }

        public int? RoleId { get; set; }

        public virtual ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public virtual ICollection<CheckupScheduleDto> CheckupScheduleCustomers { get; set; } = new List<CheckupScheduleDto>();

        public virtual ICollection<CheckupScheduleDto> CheckupScheduleDentists { get; set; } = new List<CheckupScheduleDto>();

        public virtual ClinicDto? Clinic { get; set; }

        public virtual ICollection<DentistServiceDto> DentistServices { get; set; } = new List<DentistServiceDto>();

        public virtual ICollection<DentistSlotDto> DentistSlots { get; set; } = new List<DentistSlotDto>();

        public virtual ICollection<MedicalRecordDto> MedicalRecords { get; set; } = new List<MedicalRecordDto>();

        public virtual RoleDto? Role { get; set; }
    }
}
