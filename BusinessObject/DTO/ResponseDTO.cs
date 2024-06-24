using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ResponseDTO
    {
        public object? Result {  get; set; }

        public bool IsSuccess { get; set; }

        public string? Message { get; set; }
    }
}
