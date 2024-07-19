using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class PrescriptionRepo : IPrescriptionrepo
    {
        public async Task CreatePrescription(Prescription prescription)
        => await PrescriptionDAO.Instance.createPrescription(prescription);

        public async Task DeletePrescription(int id)
        {
            var model = await PrescriptionDAO.Instance.getPrescriptionByID(id);
            await PrescriptionDAO.Instance.deletePrescription(model);
        }

        public async Task<Prescription> GetById(int id)
        => await PrescriptionDAO.Instance.getPrescriptionByID(id);

        public  async Task<Prescription> GetByIdWithMedicinesAsync(int id)
        => await PrescriptionDAO.Instance.GetByIdWithMedicinesAsync(id);

        public async Task<List<Prescription>> GetPrescriptions()
        => await PrescriptionDAO.Instance.getAllPrescriptions();
    

        public async Task UpdatePrescription(Prescription prescription)
        => await PrescriptionDAO.Instance.updatePrescription(prescription);
}
}
