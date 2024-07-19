using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class ClinicRepo : IClinicRepo
    {
        public async Task<List<Clinic>> GetAllClinics()
            => await ClinicDAO.Instance.getAllClinics();

        public async Task<Clinic> GetById(int? id)
            => await ClinicDAO.Instance.getClinicByID(id);

        public async Task CreateClinic(Clinic clinic)
            => await ClinicDAO.Instance.createClinic(clinic);

        public async Task UpdateClinic(Clinic clinic)
            => await ClinicDAO.Instance.updateClinic(clinic);

        public async Task DeleteClinic(int id)
        {
            var model = await ClinicDAO.Instance.getClinicByID(id);
            await ClinicDAO.Instance.deleteClinic(model);
        }
    }
}
