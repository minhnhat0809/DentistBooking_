using BusinessObject;
using Repository.Impl;
using Service.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public interface IPrescriptionMedicinesService
    {
        public Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines();
        public Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId( int preId);
        public Task<PrescriptionMedicine> GetById(int? id);
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public void DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
    }
    public class PrescriptionMedicinesService : IPrescriptionMedicinesService
    {
        private readonly IPrescriptionMedicineRepo _prescriptionMedicineRepo;
        public PrescriptionMedicinesService(IPrescriptionMedicineRepo prescriptionMedicineRepo)
        {
            _prescriptionMedicineRepo = prescriptionMedicineRepo;
        }
        public void UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                _prescriptionMedicineRepo.UpdatePrescriptionMedicine(prescriptionMedicine);


            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating.", ex);
            }

        }
        public void AddPrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                _prescriptionMedicineRepo.AddPrescriptionMedicine(prescriptionMedicine);

            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicinesByPrescriptionId(int preId)
        {
            
            try
            {
                var models = await _prescriptionMedicineRepo.GetAllPrescriptionMedicinesByPrescriptionId(preId);
                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }

        public async Task<PrescriptionMedicine> GetById(int? id)
        {

            try
            {
                var model = await _prescriptionMedicineRepo.GetById(id);
                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }

        public void DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                _prescriptionMedicineRepo.DeletePrescriptionMedicine(prescriptionMedicine);

            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task<List<PrescriptionMedicine>> GetAllPrescriptionMedicines()
        {
            try
            {
                var models = await _prescriptionMedicineRepo.GetAllPrescriptionMedicines();
                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }
    }
}
