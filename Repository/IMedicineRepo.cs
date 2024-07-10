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
        public Task<List<Medicine>> GetAllMedicines();
        public Task<Medicine> GetById(int? id);
        public void CreateMedicine(Medicine medicine);
        public void UpdateMedicine(Medicine medicine);
        public void DeleteMedicine(int id);
    }
}
