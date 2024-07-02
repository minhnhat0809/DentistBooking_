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
        public List<MedicalRecord> GetAllMedicalRecords()
            => MedicalRecordDAO.Instance.getAllMedicalRecords();

        public MedicalRecord GetById(int? id)
            => MedicalRecordDAO.Instance.getMedicalRecordByID(id);

        public void CreateMedicalRecord(MedicalRecord medical)
            => MedicalRecordDAO.Instance.createMedicalRecord(medical);

        public void UpdateMedicalRecord(MedicalRecord medical)
            => MedicalRecordDAO.Instance.updateMedicalRecord(medical);

        public void DeleteMedicalRecord(int id)
        {
            var model = MedicalRecordDAO.Instance.getMedicalRecordByID(id);
            MedicalRecordDAO.Instance.deleteMedicalRecord(model);
        }
    }
}
