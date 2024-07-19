using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IMedicineRepo
    {
        Task<List<Medicine>> GetAllMedicines();
        Task<Medicine> GetById(int? id);
        Task CreateMedicine(Medicine medicine);
        Task UpdateMedicine(Medicine medicine);
        Task DeleteMedicine(int id);
    }
}
