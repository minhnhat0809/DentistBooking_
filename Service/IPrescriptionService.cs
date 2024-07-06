using BusinessObject;

namespace Service
{
    public interface IPrescriptionService
    {
        public List<Prescription> GetPrescriptions();
        public Prescription GetById(int id);
        public void CreatePrescription(Prescription prescription);
        public void UpdatePrescription(Prescription prescription);
        public void DeletePrescription(int id);
        Task<Prescription> GetByIdWithMedicinesAsync(int id);

        List<Prescription> GetAllPrescriptionByCustomer(int customerId);

    }
}
