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

        public void CreateClinic(Clinic clinic)
            => ClinicDAO.Instance.createClinic(clinic);

        public void UpdateClinic(Clinic clinic)
            => ClinicDAO.Instance.updateClinic(clinic);

        public async void DeleteClinic(int id)
        {
            var model = await ClinicDAO.Instance.getClinicByID(id);
            ClinicDAO.Instance.deleteClinic(model);
        }
    }
}
