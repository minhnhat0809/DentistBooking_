using System;
using System.Collections.Generic;
using BusinessObject;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepo _medicineRepo;

        public MedicineService(IMedicineRepo medicineRepo)
        {
            _medicineRepo = medicineRepo ?? throw new ArgumentNullException(nameof(medicineRepo));
        }

        public List<Medicine> GetAllMedicines()
        {
            try
            {
                return _medicineRepo.GetAllMedicines();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving medicines.", ex);
            }
        }

        public Medicine GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medicine ID.", nameof(id));
            }

            try
            {
                var model = _medicineRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {id} not found.");
                }

                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medicine.", ex);
            }
        }

        public void CreateMedicine(Medicine medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(medicine), "Medicine cannot be null.");
            }

            try
            {
                var existingMedicine = _medicineRepo.GetById(medicine.MedicineId);
                if (existingMedicine != null)
                {
                    throw new InvalidOperationException($"Medicine with ID {medicine.MedicineId} already exists.");
                }

                _medicineRepo.CreateMedicine(medicine);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the medicine.", ex);
            }
        }

        public void UpdateMedicine(Medicine medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(medicine), "Medicine cannot be null.");
            }

            try
            {
                var existingMedicine = _medicineRepo.GetById(medicine.MedicineId);
                if (existingMedicine == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {medicine.MedicineId} not found.");
                }

                _medicineRepo.UpdateMedicine(medicine);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the medicine.", ex);
            }
        }

        public void DeleteMedicine(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medicine ID.", nameof(id));
            }

            try
            {
                var existingMedicine = _medicineRepo.GetById(id);
                if (existingMedicine == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {id} not found.");
                }

                _medicineRepo.DeleteMedicine(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the medicine.", ex);
            }
        }
    }
}
