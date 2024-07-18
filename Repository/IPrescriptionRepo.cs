using BusinessObject;

namespace Repository
{
    public interface IPrescriptionrepo
    {
        public Task<List<Prescription>> GetPrescriptions();
        public Task<Prescription> GetById(int id);
        public void CreatePrescription(Prescription prescription);
        public void UpdatePrescription(Prescription prescription);
        public void DeletePrescription(int id);
        Task<Prescription> GetByIdWithMedicinesAsync(int id);
    }
}
