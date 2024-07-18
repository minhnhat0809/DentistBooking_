using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IPrescriptionService
    {
        public Task<List<PrescriptionDto>> GetPrescriptions();
        public Task<PrescriptionDto> GetById(int id);
        public void CreatePrescription(PrescriptionDto prescription);
        public void UpdatePrescription(PrescriptionDto prescription);
        public void DeletePrescription(int id);
        Task<PrescriptionDto> GetByIdWithMedicinesAsync(int id);

        Task<List<PrescriptionDto>> GetAllPrescriptionByCustomer(int customerId);

    }
}
