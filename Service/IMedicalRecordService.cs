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
        public Task<MedicalRecord> GetById(int? id);
        public void CreateMedicalRecord(MedicalRecord medical);
        public void UpdateMedicalRecord(MedicalRecord medical);
        public void DeleteMedicalRecord(int id);
        public Task<MedicalRecordDto> GetDtoById(int? id);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByCustomerIdAsync(int customerId);
    }
}
