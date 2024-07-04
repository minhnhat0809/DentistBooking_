using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{
    public interface IClinicService
    {
        public Task<List<Clinic>> GetAllClinics();
        public Task<Clinic> GetById(int? id);
        public void CreateClinic(Clinic clinic);
        public void UpdateClinic(Clinic clinic);
        public void DeleteClinic(int id);
    }
}
