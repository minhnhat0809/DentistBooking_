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
        public List<Clinic> GetAllClinics()
            => ClinicDAO.Instance.getAllClinics();

        public Clinic GetById(int id)
            => ClinicDAO.Instance.getClinicByID(id);

        public void CreateClinic(Clinic clinic)
            => ClinicDAO.Instance.createClinic(clinic);

        public void UpdateClinic(Clinic clinic)
            => ClinicDAO.Instance.updateClinic(clinic);

        public void DeleteClinic(int id)
        {
            var model = ClinicDAO.Instance.getClinicByID(id);
            ClinicDAO.Instance.deleteClinic(model);
        }
    }
}
