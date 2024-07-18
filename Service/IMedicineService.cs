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
        public Task<List<MedicineDto>> GetAllMedicines();
        public Task<MedicineDto> GetById(int? id);
        public void  CreateMedicine(MedicineDto medicine);
        public void UpdateMedicine(MedicineDto medicine);
        public void DeleteMedicine(int id);
    }
}
