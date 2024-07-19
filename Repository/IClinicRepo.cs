using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IClinicRepo
    {
        Task<List<Clinic>> GetAllClinics();
        Task<Clinic> GetById(int? id);
        Task CreateClinic(Clinic clinic);
        Task UpdateClinic(Clinic clinic);
        Task DeleteClinic(int id);

    }
}
