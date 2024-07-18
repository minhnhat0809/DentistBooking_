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
        public Task<List<ClinicDto>> GetAllClinics();
        public Task<ClinicDto> GetById(int? id);
        public void CreateClinic(ClinicDto clinic);
        public void UpdateClinic(ClinicDto clinic);
        public void DeleteClinic(int id);
    }
}
