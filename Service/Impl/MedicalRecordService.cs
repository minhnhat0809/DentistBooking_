using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepo _medicalRecordRepo;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepo medicalRecordRepo, IMapper mapper)
        {
            _medicalRecordRepo = medicalRecordRepo ?? throw new ArgumentNullException(nameof(medicalRecordRepo));
            _mapper = mapper;
        }

        public async Task<List<MedicalRecordDto>> GetAllMedicalRecords()
        {
            try
            {
                List<MedicalRecord> models = await _medicalRecordRepo.GetAllMedicalRecords();
                var viewModels = _mapper.Map<List<MedicalRecordDto>>(models);
                
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving medical records.", ex);
            }
        }

        public async Task<MedicalRecord> GetById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var model = await _medicalRecordRepo.GetById(id);
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

        public async Task<MedicalRecordDto> GetDtoById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var model = await _medicalRecordRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {id} not found.");
                }

                return _mapper.Map<MedicalRecordDto>(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medical record.", ex);
            }
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByCustomerIdAsync(int customerId)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Invalid medical customer ID.", nameof(customerId));
            }

            try
            {
                var models = await _medicalRecordRepo.GetMedicalRecordsByCustomerIdAsync(customerId);
                if (models == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record not found.");
                }

                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medical record.", ex);
            }
        }
    }
}
