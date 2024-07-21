using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO
{
    public class ClinicDto
    {
        public int ClinicId { get; set; }

        /*[Required(ErrorMessage = "Clinic Name is required.")]
        [StringLength(100, ErrorMessage = "Clinic Name can't be longer than 100 characters.")]*/
        public string ClinicName { get; set; } = null!;

        /*[Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters.")]*/
        public string Address { get; set; } = null!;

        /*[Phone(ErrorMessage = "Invalid Phone number format.")]*/
        public string? Phone { get; set; } = null!;

        /*[EmailAddress(ErrorMessage = "Invalid Email address format.")]*/
        public string? Email { get; set; } = null!;

        public bool? Status { get; set; }

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
