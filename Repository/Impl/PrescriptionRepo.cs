using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class PrescriptionRepo : IPrescriptionrepo
    {
        public void CreatePrescription(Prescription prescription)
        => PrescriptionDAO.Instance.createPrescription(prescription);

        public void DeletePrescription(int id)
        {
            var model = PrescriptionDAO.Instance.getPrescriptionByID(id);
            PrescriptionDAO.Instance.deletePrescription(model);
        }

        public Prescription GetById(int id)
        => PrescriptionDAO.Instance.getPrescriptionByID(id);

        public  async Task<Prescription> GetByIdWithMedicinesAsync(int id)
        => await PrescriptionDAO.Instance.GetByIdWithMedicinesAsync(id);

        public List<Prescription> GetPrescriptions()
        => PrescriptionDAO.Instance.getAllPrescriptions();
    

        public void UpdatePrescription(Prescription prescription)
        => PrescriptionDAO.Instance.updatePrescription(prescription);
}
}
