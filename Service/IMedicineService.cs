using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IMedicineService
    {
        Task<List<MedicineDto>> GetAllMedicines();
        Task<MedicineDto> GetById(int? id);
        Task CreateMedicine(MedicineDto medicine);
        Task UpdateMedicine(MedicineDto medicine);
        Task DeleteMedicine(int id);
    }
}
