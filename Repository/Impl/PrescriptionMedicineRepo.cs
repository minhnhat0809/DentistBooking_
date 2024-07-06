using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public interface IPrescriptionMedicineRepo
    {
        public Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines();
        public Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int preId);
        public Task<PrescriptionMedicine> GetById(int? id);
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public void DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
    }
    public class PrescriptionMedicineRepo : IPrescriptionMedicineRepo
    {
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => PrescriptionMedicineDAO.Instance.createPrescriptionMedicine(prescriptionMedicine);

        public void DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => PrescriptionMedicineDAO.Instance.deletePrescriptionMedicine(prescriptionMedicine);

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines()
        => await PrescriptionMedicineDAO.Instance.getAllPrescriptionMedicines();

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int preId)
        => await PrescriptionMedicineDAO.Instance.GetAllPrescriptionMedicinesByPrescriptionId(preId);

        public async Task<PrescriptionMedicine> GetById(int? id)
        => await PrescriptionMedicineDAO.Instance.GetByID(id.Value);

        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => PrescriptionMedicineDAO.Instance.updatePrescriptionMedicine(prescriptionMedicine);
    }
}
