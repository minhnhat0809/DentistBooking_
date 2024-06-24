using System;
using System.Collections.Generic;
using BusinessObject;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepo _medicalRecordRepo;

        public MedicalRecordService(IMedicalRecordRepo medicalRecordRepo)
        {
            _medicalRecordRepo = medicalRecordRepo ?? throw new ArgumentNullException(nameof(medicalRecordRepo));
        }

        public List<MedicalRecord> GetAllMedicalRecords()
        {
            try
            {
                return _medicalRecordRepo.GetAllMedicalRecords();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving medical records.", ex);
            }
        }

        public MedicalRecord GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var model = _medicalRecordRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {id} not found.");
                }

                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medical record.", ex);
            }
        }

        public void CreateMedicalRecord(MedicalRecord medical)
        {
            if (medical == null)
            {
                throw new ArgumentNullException(nameof(medical), "Medical record cannot be null.");
            }

            try
            {
                var existingRecord = _medicalRecordRepo.GetById(medical.MediaRecordId);
                if (existingRecord != null)
                {
                    throw new InvalidOperationException($"Medical record with ID {medical.MediaRecordId} already exists.");
                }

                _medicalRecordRepo.CreateMedicalRecord(medical);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the medical record.", ex);
            }
        }

        public void UpdateMedicalRecord(MedicalRecord medical)
        {
            if (medical == null)
            {
                throw new ArgumentNullException(nameof(medical), "Medical record cannot be null.");
            }

            try
            {
                var existingRecord = _medicalRecordRepo.GetById(medical.MediaRecordId);
                if (existingRecord == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {medical.MediaRecordId} not found.");
                }

                _medicalRecordRepo.UpdateMedicalRecord(medical);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the medical record.", ex);
            }
        }

        public void DeleteMedicalRecord(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var existingRecord = _medicalRecordRepo.GetById(id);
                if (existingRecord == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {id} not found.");
                }

                _medicalRecordRepo.DeleteMedicalRecord(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the medical record.", ex);
            }
        }
    }
}
