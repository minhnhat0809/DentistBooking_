using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IMedicalRecordRepo
    {
        Task<List<MedicalRecord>> GetAllMedicalRecords();
        Task<MedicalRecord> GetById(int? id);
        Task CreateMedicalRecord(MedicalRecord medical);
        Task UpdateMedicalRecord(MedicalRecord medical);
        Task DeleteMedicalRecord(int id);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByCustomerIdAsync(int customerId);
    }
}
