using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IMedicalRecordService
    {
        public Task<List<MedicalRecordDto>> GetAllMedicalRecords();
        public void CreateMedicalRecord(MedicalRecordDto medical);
        public void UpdateMedicalRecord(MedicalRecordDto medical);
        public void DeleteMedicalRecord(int id);
        public Task<MedicalRecordDto> GetById(int? id);
        Task<List<MedicalRecordDto>> GetMedicalRecordsByCustomerIdAsync(int customerId);
    }
}
