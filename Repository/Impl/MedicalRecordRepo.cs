using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class MedicalRecordRepo : IMedicalRecordRepo
    {
        public Task<List<MedicalRecord>> GetAllMedicalRecords()
            => MedicalRecordDAO.Instance.getAllMedicalRecords();

        public async Task<MedicalRecord> GetById(int? id)
            => await MedicalRecordDAO.Instance.getMedicalRecordByID(id);

        public void CreateMedicalRecord(MedicalRecord medical)
            => MedicalRecordDAO.Instance.createMedicalRecord(medical);

        public void UpdateMedicalRecord(MedicalRecord medical)
            => MedicalRecordDAO.Instance.updateMedicalRecord(medical);

        public async void DeleteMedicalRecord(int id)
        {
            var model = await MedicalRecordDAO.Instance.getMedicalRecordByID(id);
            MedicalRecordDAO.Instance.deleteMedicalRecord(model);
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByCustomerIdAsync(int customerId)
        => await MedicalRecordDAO.Instance.GetMedicalRecordsByCustomerIdAsync(customerId);
    }
}
