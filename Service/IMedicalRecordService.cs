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
        Task<List<MedicalRecordDto>> GetAllMedicalRecords();
        Task CreateMedicalRecord(MedicalRecordDto medical);
        Task UpdateMedicalRecord(MedicalRecordDto medical);
        Task DeleteMedicalRecord(int id);
        Task<MedicalRecordDto> GetById(int? id);
        Task<List<MedicalRecordDto>> GetMedicalRecordsByCustomerIdAsync(int customerId);
    }
}
