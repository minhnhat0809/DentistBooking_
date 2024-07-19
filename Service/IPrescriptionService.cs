using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IPrescriptionService
    {
        Task<List<PrescriptionDto>> GetPrescriptions();
        Task<PrescriptionDto> GetById(int id);
        Task CreatePrescription(PrescriptionDto prescription);
        Task UpdatePrescription(PrescriptionDto prescription);
        Task DeletePrescription(int id);
        Task<PrescriptionDto> GetByIdWithMedicinesAsync(int id);

        Task<List<PrescriptionDto>> GetAllPrescriptionByCustomer(int customerId);

    }
}
