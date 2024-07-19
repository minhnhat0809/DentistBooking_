using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class MedicineRepo : IMedicineRepo
    {
        public async Task<List<Medicine>> GetAllMedicines()
            => await MedicineDAO.Instance.getAllMedicines();

        public async Task<Medicine> GetById(int? id)
            => await MedicineDAO.Instance.getMedicineByID(id);

        public async Task CreateMedicine(Medicine medicine)
            => await MedicineDAO.Instance.createMedicine(medicine);

        public async Task UpdateMedicine(Medicine medicine)
            => await MedicineDAO.Instance.updateMedicine(medicine);

        public async Task DeleteMedicine(int id)
        {
            var model = await MedicineDAO.Instance.getMedicineByID(id);
            await MedicineDAO.Instance.deleteMedicine(model);
        }
    }
}
