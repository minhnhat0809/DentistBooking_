using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public interface IPrescriptionMedicineRepo
    {
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
    }
    public class PrescriptionMedicineRepo : IPrescriptionMedicineRepo
    {
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => PrescriptionMedicineDAO.Instance.createPrescriptionMedicine(prescriptionMedicine);

        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        => PrescriptionMedicineDAO.Instance.updatePrescriptionMedicine(prescriptionMedicine);
    }
}
