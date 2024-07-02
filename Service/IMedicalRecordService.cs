using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{
    public interface IMedicalRecordService
    {
        public List<MedicalRecord> GetAllMedicalRecords();
        public MedicalRecord GetById(int? id);
        public void CreateMedicalRecord(MedicalRecord medical);
        public void UpdateMedicalRecord(MedicalRecord medical);
        public void DeleteMedicalRecord(int id);
    }
}
