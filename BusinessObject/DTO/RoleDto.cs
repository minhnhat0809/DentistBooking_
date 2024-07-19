using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class RoleDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
