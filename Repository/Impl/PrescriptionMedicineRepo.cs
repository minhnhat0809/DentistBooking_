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
        Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines();
        Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int preId);
        Task<PrescriptionMedicine> GetById(int? id);
        Task AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        Task DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        Task UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
    }
    public class PrescriptionMedicineRepo : IPrescriptionMedicineRepo
    {
        public async Task AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => await PrescriptionMedicineDAO.Instance.createPrescriptionMedicine(prescriptionMedicine);

        public async Task DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => await PrescriptionMedicineDAO.Instance.deletePrescriptionMedicine(prescriptionMedicine);

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines()
        => await PrescriptionMedicineDAO.Instance.getAllPrescriptionMedicines();

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int preId)
        => await PrescriptionMedicineDAO.Instance.GetAllPrescriptionMedicinesByPrescriptionId(preId);

        public async Task<PrescriptionMedicine> GetById(int? id)
        => await PrescriptionMedicineDAO.Instance.GetByID(id.Value);

        public async Task UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => await PrescriptionMedicineDAO.Instance.updatePrescriptionMedicine(prescriptionMedicine);
    }
}
