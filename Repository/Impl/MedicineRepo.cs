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

        public void CreateMedicine(Medicine medicine)
            => MedicineDAO.Instance.createMedicine(medicine);

        public void UpdateMedicine(Medicine medicine)
            => MedicineDAO.Instance.updateMedicine(medicine);

        public void DeleteMedicine(int id)
        {
            var model = MedicineDAO.Instance.getMedicineByID(id).Result;
            MedicineDAO.Instance.deleteMedicine(model);
        }
    }
}
