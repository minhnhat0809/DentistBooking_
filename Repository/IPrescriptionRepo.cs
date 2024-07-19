using BusinessObject;

namespace Repository
{
    public interface IPrescriptionrepo
    {
        Task<List<Prescription>> GetPrescriptions();
        Task<Prescription> GetById(int id);
        Task CreatePrescription(Prescription prescription);
        Task UpdatePrescription(Prescription prescription);
        Task DeletePrescription(int id);
        Task<Prescription> GetByIdWithMedicinesAsync(int id);
    }
}
