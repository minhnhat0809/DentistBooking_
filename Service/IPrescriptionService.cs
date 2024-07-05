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

        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);

        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
    }
}
