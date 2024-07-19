using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IClinicService
    {
        Task<List<ClinicDto>> GetAllClinics();
        Task<ClinicDto> GetById(int? id);
        Task CreateClinic(ClinicDto clinic);
        Task UpdateClinic(ClinicDto clinic);
        Task DeleteClinic(int id);
    }
}
