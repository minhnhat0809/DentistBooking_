using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

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
