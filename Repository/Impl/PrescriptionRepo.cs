using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class PrescriptionRepo : IPrescriptionrepo
    {
        public void CreatePrescription(Prescription prescription)
        => PrescriptionDAO.Instance.createPrescription(prescription);

        public async void DeletePrescription(int id)
        {
            var model = await PrescriptionDAO.Instance.getPrescriptionByID(id);
            PrescriptionDAO.Instance.deletePrescription(model);
        }

        public async Task<Prescription> GetById(int id)
        => await PrescriptionDAO.Instance.getPrescriptionByID(id);

        public  async Task<Prescription> GetByIdWithMedicinesAsync(int id)
        => await PrescriptionDAO.Instance.GetByIdWithMedicinesAsync(id);

        public async Task<List<Prescription>> GetPrescriptions()
        => await PrescriptionDAO.Instance.getAllPrescriptions();
    

        public void UpdatePrescription(Prescription prescription)
        => PrescriptionDAO.Instance.updatePrescription(prescription);
}
}
